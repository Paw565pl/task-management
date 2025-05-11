using TaskManagement.Backend.Features.Task.Entity;
using TaskStatus = TaskManagement.Backend.Features.Task.Entity.TaskStatus;

namespace TaskManagement.Backend.Features.Task.Dto;

public record TaskFilterDto(
    TaskStatus? Status,
    TaskPriority? Priority,
    DateOnly? DueDateAfter,
    DateOnly? DueDateBefore
);
