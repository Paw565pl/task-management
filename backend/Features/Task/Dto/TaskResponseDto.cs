using TaskManagement.Backend.Features.Task.Entity;
using TaskStatus = TaskManagement.Backend.Features.Task.Entity.TaskStatus;

namespace TaskManagement.Backend.Features.Task.Dto;

public record TaskResponseDto(
    long Id,
    string Title,
    string? Description,
    TaskStatus Status,
    TaskPriority Priority,
    DateOnly DueDate,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
