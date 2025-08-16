using System.Collections.ObjectModel;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using TaskManagement.Backend.Core.DbContext;
using TaskManagement.Backend.Core.Dto;
using TaskManagement.Backend.Core.ExceptionHandler;
using TaskManagement.Backend.Features.Project.Dto;
using TaskManagement.Backend.Features.Project.Entity;
using TaskManagement.Backend.Features.Project.Exception;
using TaskManagement.Backend.Features.Project.Mapper;

namespace TaskManagement.Backend.Features.Project.Service;

public class ProjectService(AppDbContext appDbContext)
{
    private static readonly ReadOnlyDictionary<string, Expression<Func<ProjectEntity, object>>> SortColumns =
        new Dictionary<string, Expression<Func<ProjectEntity, object>>>(StringComparer.OrdinalIgnoreCase)
        {
            ["name"] = x => x.Name,
            ["createdat"] = x => x.CreatedAt,
            ["updatedat"] = x => x.UpdatedAt,
        }.AsReadOnly();

    public async Task<PageResponseDto<ProjectResponseDto>> GetAllAsync(SortOptionsDto? sortOptionsDto = null,
        PageOptionsDto? pageOptionsDto = null, CancellationToken cancellationToken = default)
    {
        var query = appDbContext.Projects.AsNoTracking();

        var pageNumber = pageOptionsDto?.PageNumber ?? PageOptionsDto.DefaultPageNumber;
        var pageSize = pageOptionsDto?.PageSize ?? PageOptionsDto.DefaultPageSize;

        var total = await query.CountAsync(cancellationToken);
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
        var content = await query.ToResponseDto().ToListAsync(cancellationToken);

        return new(content, total, pageNumber, pageSize);
    }

    public async Task<ProjectResponseDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var project = await appDbContext.Projects.AsNoTracking().Where(p => p.Id == id)
            .ToResponseDto().FirstOrDefaultAsync(cancellationToken);
        if (project is null) throw new ProblemDetailsException(ProjectExceptionReason.NotFound);

        return project;
    }

    public async Task<ProjectResponseDto> CreateAsync(ProjectCreateRequestDto projectCreateRequestDto, CancellationToken cancellationToken = default)
    {
        try
        {
            var project = projectCreateRequestDto.ToEntity();

            await appDbContext.Projects.AddAsync(project, cancellationToken);
            await appDbContext.SaveChangesAsync(cancellationToken);

            return project.ToResponseDto();
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException { SqlState: "23505" } postgresEx &&
                                           string.Equals(postgresEx.ConstraintName, ProjectEntity.UniqueNameConstraint))
        {
            throw new ProblemDetailsException(ProjectExceptionReason.NameNotUnique);
        }
    }
}
