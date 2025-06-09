using Microsoft.EntityFrameworkCore;
using TaskManagement.Backend.Core.Context;
using TaskManagement.Backend.Core.Dto;
using TaskManagement.Backend.Core.ExceptionHandler;
using TaskManagement.Backend.Features.Project.Exception;
using TaskManagement.Backend.Features.Task.Dto;
using TaskManagement.Backend.Features.Task.Exception;
using TaskManagement.Backend.Features.Task.Mapper;

namespace TaskManagement.Backend.Features.Task.Service;

public class TaskService(AppDbContext appDbContext)
{
    public async Task<PageResponseDto<TaskResponseDto>> GetAllAsync(long projectId, TaskFilterDto? taskFilterDto,
        SortOptionsDto? sortOptionsDto,
        PageOptionsDto? pageOptionsDto)
    {
        var doesProjectExist =
            await appDbContext.Projects.AsNoTracking().AnyAsync(p => p.Id == projectId);
        if (!doesProjectExist) throw new ProblemDetailsException(ProjectExceptionReason.NotFound);

        var query = appDbContext.Tasks.AsNoTracking();

        if (taskFilterDto?.Status is not null) query = query.Where(t => t.Status == taskFilterDto.Status);
        if (taskFilterDto?.Priority is not null) query = query.Where(t => t.Priority == taskFilterDto.Priority);
        if (taskFilterDto?.DueDateAfter is not null) query = query.Where(t => t.DueDate >= taskFilterDto.DueDateAfter);
        if (taskFilterDto?.DueDateBefore is not null)
            query = query.Where(t => t.DueDate <= taskFilterDto.DueDateBefore);

        var sortField = sortOptionsDto?.SortBy?.ToLower();
        var sortDirection = sortOptionsDto?.SortDirection;
        query = sortField switch
        {
            "status" => sortDirection == SortDirection.Asc
                ? query.OrderBy(t => t.Status).ThenBy(t => t.Id)
                : query.OrderByDescending(t => t.Status).ThenBy(t => t.Id),
            "priority" => sortDirection == SortDirection.Asc
                ? query.OrderBy(t => t.Priority).ThenBy(t => t.Id)
                : query.OrderByDescending(t => t.Priority).ThenBy(t => t.Id),
            "duedate" => sortDirection == SortDirection.Asc
                ? query.OrderBy(t => t.DueDate).ThenBy(t => t.Id)
                : query.OrderByDescending(t => t.DueDate).ThenBy(t => t.Id),
            "createdat" => sortDirection == SortDirection.Asc
                ? query.OrderBy(t => t.CreatedAt).ThenBy(t => t.Id)
                : query.OrderByDescending(t => t.CreatedAt).ThenBy(t => t.Id),
            "updatedat" => sortDirection == SortDirection.Asc
                ? query.OrderBy(t => t.UpdatedAt).ThenBy(t => t.Id)
                : query.OrderByDescending(t => t.UpdatedAt).ThenBy(t => t.Id),
            _ => query.OrderBy(t => t.Id)
        };

        var pageNumber = pageOptionsDto?.PageNumber ?? 1;
        var pageSize = pageOptionsDto?.PageSize ?? 20;

        var total = await query.CountAsync();
        if (total == 0) return new([], total, pageNumber, pageSize);

        query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        var content = await query.ToResponseDto().ToListAsync();

        return new(content, total, pageNumber, pageSize);
    }

    public async Task<TaskResponseDto> GetByIdAsync(long projectId, long taskId)
    {
        var doesProjectExist =
            await appDbContext.Projects.AsNoTracking().AnyAsync(p => p.Id == projectId);
        if (!doesProjectExist) throw new ProblemDetailsException(ProjectExceptionReason.NotFound);

        var task = await appDbContext.Tasks.AsNoTracking().Where(t => t.Id == taskId)
            .ToResponseDto()
            .FirstOrDefaultAsync();
        if (task is null) throw new ProblemDetailsException(TaskExceptionReason.NotFound);

        return task;
    }

    public async Task<TaskResponseDto> CreateAsync(long projectId, TaskCreateRequestDto taskCreateRequestDto)
    {
        var project = await appDbContext.Projects
            .FirstOrDefaultAsync(p => p.Id == projectId);
        if (project is null) throw new ProblemDetailsException(ProjectExceptionReason.NotFound);
        project.UpdatedAt = DateTime.UtcNow;

        var task = TaskMapper.ToEntity(taskCreateRequestDto);
        task.Project = project;

        await appDbContext.Tasks.AddAsync(task);
        await appDbContext.SaveChangesAsync();

        return TaskMapper.ToResponseDto(task);
    }

    public async Task<TaskResponseDto> UpdateAsync(long projectId, long taskId,
        TaskUpdateRequestDto taskUpdateRequestDto)
    {
        var project = await appDbContext.Projects
            .FirstOrDefaultAsync(p => p.Id == projectId);
        if (project is null) throw new ProblemDetailsException(ProjectExceptionReason.NotFound);
        project.UpdatedAt = DateTime.UtcNow;

        var doesTaskExist = await appDbContext.Tasks.AsNoTracking().AnyAsync(t => t.Id == taskId);
        if (!doesTaskExist) throw new ProblemDetailsException(TaskExceptionReason.NotFound);

        var task = TaskMapper.ToEntity(taskUpdateRequestDto);
        task.Id = taskId;
        task.Project = project;

        await appDbContext.SaveChangesAsync();

        return TaskMapper.ToResponseDto(task);
    }

    public async Task<TaskResponseDto> UpdateStatusAsync(long projectId, long taskId,
        TaskUpdateStatusRequestDto taskUpdateStatusRequestDto)
    {
        var project = await appDbContext.Projects
            .FirstOrDefaultAsync(p => p.Id == projectId);
        if (project is null) throw new ProblemDetailsException(ProjectExceptionReason.NotFound);
        project.UpdatedAt = DateTime.UtcNow;

        var task = await appDbContext.Tasks.FirstOrDefaultAsync(t => t.Id == taskId);
        if (task is null) throw new ProblemDetailsException(TaskExceptionReason.NotFound);

        task.Status = taskUpdateStatusRequestDto.Status;
        await appDbContext.SaveChangesAsync();

        return TaskMapper.ToResponseDto(task);
    }

    public async System.Threading.Tasks.Task DeleteAsync(long projectId, long taskId)
    {
        var project = await appDbContext.Projects
            .FirstOrDefaultAsync(p => p.Id == projectId);
        if (project is null) throw new ProblemDetailsException(ProjectExceptionReason.NotFound);
        project.UpdatedAt = DateTime.UtcNow;

        var task = await appDbContext.Tasks.FirstOrDefaultAsync(t => t.Id == taskId);
        if (task is null) throw new ProblemDetailsException(TaskExceptionReason.NotFound);

        appDbContext.Tasks.Remove(task);
        await appDbContext.SaveChangesAsync();
    }
}
