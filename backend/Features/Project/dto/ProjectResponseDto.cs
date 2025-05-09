namespace TaskManagement.Backend.Features.Project.dto;

public record ProjectResponseDto(
    long Id,
    string Name,
    string? Description,
    int TaskCount,
    DateTime? CreatedAt,
    DateTime? UpdatedAt
);
