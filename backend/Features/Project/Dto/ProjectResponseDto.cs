namespace TaskManagement.Backend.Features.Project.Dto;

public record ProjectResponseDto(
    long Id,
    string Name,
    string? Description,
    int TaskCount,
    int CompletedTaskCount,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
