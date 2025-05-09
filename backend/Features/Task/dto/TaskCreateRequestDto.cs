using TaskManagement.Backend.Features.Task.Entity;

namespace TaskManagement.Backend.Features.Task.dto;

public record TaskCreateRequestDto(
    string Title,
    string? Description,
    TaskPriority TaskPriority,
    DateOnly DueDate
);
