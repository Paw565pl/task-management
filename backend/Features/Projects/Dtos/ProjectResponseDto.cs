namespace TaskManagement.Backend.Features.Projects.Dtos;

public record ProjectResponseDto(
    long Id,
    string Name,
    string? Description,
    int TaskCount,
    int CompletedTaskCount,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
