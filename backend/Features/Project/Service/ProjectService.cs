using Microsoft.EntityFrameworkCore;
using TaskManagement.Backend.Core.Context;
using TaskManagement.Backend.Core.Dto;
using TaskManagement.Backend.Core.ExceptionHandler;
using TaskManagement.Backend.Features.Project.Dto;
using TaskManagement.Backend.Features.Project.Exception;
using TaskManagement.Backend.Features.Project.Mapper;

namespace TaskManagement.Backend.Features.Project.Service;

public class ProjectService(AppDbContext appDbContext)
{
    public async Task<PageResponseDto<ProjectResponseDto>> GetAllAsync(SortOptionsDto? sortOptionsDto,
        PageOptionsDto? pageOptionsDto)
    {
        var query = appDbContext.Projects.AsNoTracking();

        var sortField = sortOptionsDto?.SortBy?.ToLower();
        var sortDirection = sortOptionsDto?.SortDirection;
        query = sortField switch
        {
            "name" => sortDirection == SortDirection.Asc
                ? query.OrderBy(p => p.Name).ThenBy(p => p.Id)
                : query.OrderByDescending(p => p.Name).ThenBy(p => p.Id),
            "created_at" => sortDirection == SortDirection.Asc
                ? query.OrderBy(p => p.CreatedAt).ThenBy(p => p.Id)
                : query.OrderByDescending(p => p.CreatedAt).ThenBy(p => p.Id),
            "updated_at" => sortDirection == SortDirection.Asc
                ? query.OrderBy(p => p.UpdatedAt).ThenBy(p => p.Id)
                : query.OrderByDescending(p => p.UpdatedAt).ThenBy(p => p.Id),
            _ => query.OrderBy(p => p.Id)
        };

        var total = await query.CountAsync();

        var pageNumber = pageOptionsDto?.PageNumber ?? 1;
        var pageSize = pageOptionsDto?.PageSize ?? 20;
        query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

        var content = await query.ToResponseDto().ToListAsync();

        return new(content, total, pageNumber, pageSize);
    }

    public async Task<ProjectResponseDto> GetByIdAsync(long id)
    {
        var project = await appDbContext.Projects.AsNoTracking().Where(p => p.Id == id)
            .ToResponseDto().FirstOrDefaultAsync();
        if (project is null) throw new ProblemDetailsException(ProjectExceptionReason.NotFound);

        return project;
    }

    public async Task<ProjectResponseDto> CreateAsync(ProjectRequestDto projectRequestDto)
    {
        try
        {
            var project = ProjectMapper.ToEntity(projectRequestDto);
            var savedProject = await appDbContext.Projects.AddAsync(project);
            await appDbContext.SaveChangesAsync();

            return ProjectMapper.ToResponseDto(savedProject.Entity);
        }
        catch (DbUpdateException)
        {
            throw new ProblemDetailsException(ProjectExceptionReason.NameNotUnique);
        }
    }
}
