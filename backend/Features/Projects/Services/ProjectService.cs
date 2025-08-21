using System.Collections.ObjectModel;
using System.Linq.Expressions;
using EntityFramework.Exceptions.Common;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Backend.Core.Db;
using TaskManagement.Backend.Core.Dtos;
using TaskManagement.Backend.Features.Projects.Dtos;
using TaskManagement.Backend.Features.Projects.Entities;
using TaskManagement.Backend.Features.Projects.Exceptions;
using TaskManagement.Backend.Features.Projects.Mappers;
using ZiggyCreatures.Caching.Fusion;

namespace TaskManagement.Backend.Features.Projects.Services;

public class ProjectService(AppDbContext appDbContext, IFusionCache fusionCache)
{
    private static readonly IEnumerable<string> ProjectCacheTag = ["project"];

    private static readonly ReadOnlyDictionary<
        string,
        Expression<Func<ProjectEntity, object>>
    > SortColumns = new Dictionary<string, Expression<Func<ProjectEntity, object>>>(
        StringComparer.OrdinalIgnoreCase
    )
    {
        ["name"] = x => x.Name,
        ["createdat"] = x => x.CreatedAt,
        ["updatedat"] = x => x.UpdatedAt,
    }.AsReadOnly();

    public async Task<PageResponseDto<ProjectResponseDto>> GetAllAsync(
        ProjectFiltersDto? projectFiltersDto = null,
        SortOptionsDto? sortOptionsDto = null,
        PageOptionsDto? pageOptionsDto = null,
        CancellationToken cancellationToken = default
    )
    {
        var cacheKey = $"projects:{projectFiltersDto}:{sortOptionsDto}:{pageOptionsDto}";

        return await fusionCache.GetOrSetAsync<PageResponseDto<ProjectResponseDto>>(
            cacheKey,
            async _ =>
            {
                var query = appDbContext.Projects.AsNoTracking();

                var searchTerm = projectFiltersDto?.SearchTerm;
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    query = query.Where(p =>
                        p.SearchVector.Matches(EF.Functions.PlainToTsQuery("english", searchTerm))
                    );
                }

                var pageNumber = pageOptionsDto?.PageNumber ?? PageOptionsDto.DefaultPageNumber;
                var pageSize = pageOptionsDto?.PageSize ?? PageOptionsDto.DefaultPageSize;

                var total = await query.CountAsync(cancellationToken);
                if (total == 0)
                    return new([], total, pageNumber, pageSize);

                if (
                    SortColumns.TryGetValue(
                        sortOptionsDto?.SortBy ?? string.Empty,
                        out var keySelector
                    )
                )
                {
                    var isAscending = sortOptionsDto?.SortDirection == SortDirection.Asc;
                    query = isAscending
                        ? query.OrderBy(keySelector).ThenBy(p => p.Id)
                        : query.OrderByDescending(keySelector).ThenBy(p => p.Id);
                }
                else if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    query = query
                        .OrderByDescending(p =>
                            p.SearchVector.Rank(EF.Functions.PlainToTsQuery("english", searchTerm))
                        )
                        .ThenBy(p => p.Id);
                }
                else
                {
                    query = query.OrderBy(p => p.Id);
                }

                query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
                var content = await query.ToResponseDto().ToListAsync(cancellationToken);

                return new(content, total, pageNumber, pageSize);
            },
            tags: ProjectCacheTag,
            token: cancellationToken
        );
    }

    public async Task<ProjectResponseDto> GetByIdAsync(
        long id,
        CancellationToken cancellationToken = default
    )
    {
        var cacheKey = $"project:{id}";

        var project = await fusionCache.GetOrSetAsync(
            cacheKey,
            async _ =>
                await appDbContext
                    .Projects.AsNoTracking()
                    .Where(p => p.Id == id)
                    .ToResponseDto()
                    .FirstOrDefaultAsync(cancellationToken),
            tags: ProjectCacheTag,
            token: cancellationToken
        );
        if (project is null)
            throw new ProjectNotFoundException();

        return project;
    }

    public async Task<ProjectResponseDto> CreateAsync(
        ProjectCreateRequestDto projectCreateRequestDto,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            var project = projectCreateRequestDto.ToEntity();

            await appDbContext.Projects.AddAsync(project, cancellationToken);
            await appDbContext.SaveChangesAsync(cancellationToken);

            await fusionCache.RemoveByTagAsync(ProjectCacheTag, token: cancellationToken);

            return project.ToResponseDto();
        }
        catch (UniqueConstraintException ex)
            when (string.Equals(
                    ex.ConstraintName,
                    ProjectEntity.UniqueNameConstraint,
                    StringComparison.Ordinal
                )
            )
        {
            throw new ProjectNameNotUniqueException();
        }
    }
}
