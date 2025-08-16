using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Backend.Core.DbContext;
using TaskManagement.Backend.Core.Dto;
using TaskManagement.Backend.Core.ExceptionHandler;
using TaskManagement.Backend.Features.Project.Exception;
using TaskManagement.Backend.Features.Task.Dto;
using TaskManagement.Backend.Features.Task.Entity;
using TaskManagement.Backend.Features.Task.Exception;
using TaskManagement.Backend.Features.Task.Mapper;

namespace TaskManagement.Backend.Features.Task.Service;

public class TaskService(AppDbContext appDbContext)
{
    private static readonly IReadOnlyDictionary<string, Expression<Func<TaskEntity, object>>> SortColumns =
        new Dictionary<string, Expression<Func<TaskEntity, object>>>(StringComparer.OrdinalIgnoreCase)
        {
            ["title"] = x => x.Title,
            ["status"] = x => x.Status,
            ["priority"] = x => x.Priority,
            ["duedate"] = x => x.DueDate,
            ["createdat"] = x => x.CreatedAt,
            ["updatedat"] = x => x.UpdatedAt,
        }.AsReadOnly();

    public async Task<PageResponseDto<TaskResponseDto>> GetAllAsync(long projectId, TaskFilterDto? taskFilterDto,
        SortOptionsDto? sortOptionsDto = null,
        PageOptionsDto? pageOptionsDto = null,
        CancellationToken cancellationToken = default)
    {
        var doesProjectExist =
            await appDbContext.Projects.AsNoTracking().AnyAsync(p => p.Id == projectId, cancellationToken);
        if (!doesProjectExist) throw new ProblemDetailsException(ProjectExceptionReason.NotFound);

        var query = appDbContext.Tasks.AsNoTracking().Where(t => t.ProjectId == projectId);

        if (taskFilterDto?.Status is not null) query = query.Where(t => t.Status == taskFilterDto.Status);
        if (taskFilterDto?.Priority is not null) query = query.Where(t => t.Priority == taskFilterDto.Priority);
        if (taskFilterDto?.DueDateAfter is not null) query = query.Where(t => t.DueDate >= taskFilterDto.DueDateAfter);
        if (taskFilterDto?.DueDateBefore is not null)
            query = query.Where(t => t.DueDate <= taskFilterDto.DueDateBefore);

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

    public async Task<TaskResponseDto> GetByIdAsync(long projectId, long taskId, CancellationToken cancellationToken = default)
    {
        var doesProjectExist =
            await appDbContext.Projects.AsNoTracking().AnyAsync(p => p.Id == projectId, cancellationToken);
        if (!doesProjectExist) throw new ProblemDetailsException(ProjectExceptionReason.NotFound);

        var task = await appDbContext.Tasks.AsNoTracking().Where(t => t.Id == taskId && t.ProjectId == projectId)
            .ToResponseDto()
            .FirstOrDefaultAsync(cancellationToken);
        if (task is null) throw new ProblemDetailsException(TaskExceptionReason.NotFound);

        return task;
    }

    public async Task<TaskResponseDto> CreateAsync(long projectId, TaskCreateRequestDto taskCreateRequestDto, CancellationToken cancellationToken = default)
    {
        var project = await appDbContext.Projects
            .FirstOrDefaultAsync(p => p.Id == projectId, cancellationToken);
        if (project is null) throw new ProblemDetailsException(ProjectExceptionReason.NotFound);

        project.RefreshUpdatedAt();

        var task = taskCreateRequestDto.ToEntity(project);

        await appDbContext.Tasks.AddAsync(task, cancellationToken);
        await appDbContext.SaveChangesAsync(cancellationToken);

        return task.ToResponseDto();
    }

    public async Task<TaskResponseDto> UpdateAsync(long projectId, long taskId,
        TaskUpdateRequestDto taskUpdateRequestDto, CancellationToken cancellationToken = default)
    {
        var project = await appDbContext.Projects
            .FirstOrDefaultAsync(p => p.Id == projectId, cancellationToken);
        if (project is null) throw new ProblemDetailsException(ProjectExceptionReason.NotFound);

        project.RefreshUpdatedAt();

        var task = await appDbContext.Tasks.FirstOrDefaultAsync(t => t.Id == taskId && t.ProjectId == project.Id, cancellationToken);
        if (task is null) throw new ProblemDetailsException(TaskExceptionReason.NotFound);

        task.Title = taskUpdateRequestDto.Title;
        task.Description = taskUpdateRequestDto.Description;
        task.Status = taskUpdateRequestDto.Status;
        task.Priority = taskUpdateRequestDto.Priority;
        task.DueDate = taskUpdateRequestDto.DueDate;

        task.RefreshUpdatedAt();
        await appDbContext.SaveChangesAsync(cancellationToken);

        return task.ToResponseDto();
    }

    public async Task<TaskResponseDto> UpdateStatusAsync(long projectId, long taskId,
        TaskUpdateStatusRequestDto taskUpdateStatusRequestDto, CancellationToken cancellationToken = default)
    {
        var project = await appDbContext.Projects
            .FirstOrDefaultAsync(p => p.Id == projectId, cancellationToken);
        if (project is null) throw new ProblemDetailsException(ProjectExceptionReason.NotFound);

        project.RefreshUpdatedAt();

        var task = await appDbContext.Tasks.FirstOrDefaultAsync(t => t.Id == taskId && t.ProjectId == project.Id, cancellationToken);
        if (task is null) throw new ProblemDetailsException(TaskExceptionReason.NotFound);

        task.Status = taskUpdateStatusRequestDto.Status;

        task.RefreshUpdatedAt();
        await appDbContext.SaveChangesAsync(cancellationToken);

        return task.ToResponseDto();
    }

    public async System.Threading.Tasks.Task DeleteAsync(long projectId, long taskId, CancellationToken cancellationToken = default)
    {
        var project = await appDbContext.Projects
            .FirstOrDefaultAsync(p => p.Id == projectId, cancellationToken);
        if (project is null) throw new ProblemDetailsException(ProjectExceptionReason.NotFound);

        await using var transaction = await appDbContext.Database.BeginTransactionAsync(cancellationToken);
        project.RefreshUpdatedAt();

        var deletedCount = await appDbContext.Tasks.Where(t => t.Id == taskId && t.ProjectId == project.Id).ExecuteDeleteAsync(cancellationToken);
        if (deletedCount == 0) throw new ProblemDetailsException(TaskExceptionReason.NotFound);

        await appDbContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
    }
}
