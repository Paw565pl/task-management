using TaskManagement.Backend.Features.Tasks.Entities;
using TaskStatus = TaskManagement.Backend.Features.Tasks.Entities.TaskStatus;

namespace TaskManagement.Backend.Features.Tasks.Dtos;

public record TaskFiltersDto(
    TaskStatus? Status,
    TaskPriority? Priority,
    DateOnly? DueDateAfter,
    DateOnly? DueDateBefore
);
