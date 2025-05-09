using TaskManagement.Backend.Features.Task.Entity;
using TaskStatus = System.Threading.Tasks.TaskStatus;

public record TaskResponseDto(
    long Id,
    string Title,
    string? Description,
    TaskStatus TaskStatus,
    TaskPriority TaskPriority,
    DateOnly DueDate,
    DateTime? CreatedAt,
    DateTime? UpdatedAt
);
