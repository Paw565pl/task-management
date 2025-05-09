using TaskManagement.Backend.Features.Task.Entity;

namespace TaskManagement.Backend.Features.Task.Dto;

public record TaskCreateRequestDto(
    string Title,
    string? Description,
    TaskPriority TaskPriority,
    DateOnly DueDate
);
