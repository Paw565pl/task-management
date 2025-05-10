using Microsoft.EntityFrameworkCore;
using TaskManagement.Backend.Core.Context;
using TaskManagement.Backend.Core.Dto;
using TaskManagement.Backend.Features.Project.Dto;
using TaskManagement.Backend.Features.Project.Mapper;
using TaskStatus = TaskManagement.Backend.Features.Task.Entity.TaskStatus;

namespace TaskManagement.Backend.Features.Project.Service;

public class ProjectService(AppDbContext appDbContext)
{
    public async Task<PageResponseDto<ProjectResponseDto>> GetAllAsync(SortOptionsDto? sortOptionsDto, PageOptionsDto? pageOptionsDto)
    {
        var query = appDbContext.ProjectEntities.AsNoTracking();

        if (string.IsNullOrWhiteSpace(sortOptionsDto?.SortBy)) query = query.OrderBy(w => w.Id);
        else
        {
            var sortField = sortOptionsDto.SortBy.ToLower();
            query = sortField switch
            {
                "name" => sortOptionsDto.SortDirection == SortDirection.Asc
                    ? query.OrderBy(w => w.Name)
                    : query.OrderByDescending(w => w.Name),
                "createdat" => sortOptionsDto.SortDirection == SortDirection.Asc
                    ? query.OrderBy(w => w.CreatedAt)
                    : query.OrderByDescending(w => w.CreatedAt),
                "UpdatedAt" => sortOptionsDto.SortDirection == SortDirection.Asc
                    ? query.OrderBy(w => w.UpdatedAt)
                    : query.OrderByDescending(w => w.UpdatedAt),
                _ => query.OrderBy(w => w.Id)
            };
        }

        var total = await query.CountAsync();

        var pageNumber = pageOptionsDto?.PageNumber >= 1 ? pageOptionsDto.PageNumber : 1;
        var pageSize = pageOptionsDto?.PageSize <= 100 ? pageOptionsDto.PageSize : 20;
        query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

        var content = await query.ToResponseDto().ToListAsync();

        return new(content, total, pageNumber, pageSize);
    }

    public async Task<ProjectResponseDto?> GetByIdAsync(long id)
    {
        return await appDbContext.ProjectEntities.AsNoTracking().Where(project => project.Id == id).ToResponseDto().FirstOrDefaultAsync();
    }

    public async Task<ProjectResponseDto> CreateAsync(ProjectRequestDto projectRequestDto)
    {
        var project = ProjectMapper.ToEntity(projectRequestDto);
        var savedProject = await appDbContext.ProjectEntities.AddAsync(project);
        await appDbContext.SaveChangesAsync();

        return ProjectMapper.ToResponseDto(savedProject.Entity);
    }
}
