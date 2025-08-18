using TaskManagement.Backend.Features.Tasks.Entities;
using TaskStatus = TaskManagement.Backend.Features.Tasks.Entities.TaskStatus;

namespace TaskManagement.Backend.Features.Tasks.Dtos;

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
