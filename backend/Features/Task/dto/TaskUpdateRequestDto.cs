using TaskManagement.Backend.Features.Task.Entity;
using TaskStatus = System.Threading.Tasks.TaskStatus;

namespace TaskManagement.Backend.Features.Task.dto;

public record TaskUpdateRequestDto(
    string Title,
    string? Description,
    TaskStatus TaskStatus,
    TaskPriority TaskPriority,
    DateOnly DueDate);
