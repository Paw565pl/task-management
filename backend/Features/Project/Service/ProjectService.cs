using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Backend.Core.Context;
using TaskManagement.Backend.Core.Dto;
using TaskManagement.Backend.Core.ExceptionHandler;
using TaskManagement.Backend.Features.Project.Dto;
using TaskManagement.Backend.Features.Project.Entity;
using TaskManagement.Backend.Features.Project.Exception;
using TaskManagement.Backend.Features.Project.Mapper;

namespace TaskManagement.Backend.Features.Project.Service;

public class ProjectService(AppDbContext appDbContext)
{
    private static readonly Dictionary<string, Expression<Func<ProjectEntity, object>>> SortColumns =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["name"] = x => x.Name,
            ["createdat"] = x => x.CreatedAt,
            ["updatedat"] = x => x.UpdatedAt,
        };

    public async Task<PageResponseDto<ProjectResponseDto>> GetAllAsync(SortOptionsDto? sortOptionsDto = null,
        PageOptionsDto? pageOptionsDto = null)
    {
        var query = appDbContext.Projects.AsNoTracking();

        var pageNumber = pageOptionsDto?.PageNumber ?? 1;
        var pageSize = pageOptionsDto?.PageSize ?? 20;

        var total = await query.CountAsync();
        if (total == 0) return new([], total, pageNumber, pageSize);

        if (SortColumns.TryGetValue(sortOptionsDto?.SortBy ?? string.Empty, out var keySelector))
        {
            query =
                sortOptionsDto?.SortDirection == SortDirection.Asc
                    ? query.OrderBy(keySelector).ThenBy(p => p.Id)
                    : query.OrderByDescending(keySelector).ThenBy(p => p.Id);
        }
        else
        {
            query = query.OrderBy(p => p.Id);
        }

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
