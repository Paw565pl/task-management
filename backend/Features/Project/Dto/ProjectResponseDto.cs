namespace TaskManagement.Backend.Features.Project.Dto;

public record ProjectResponseDto(
    long Id,
    string Name,
    string? Description,
    int TaskCount,
    DateTime? CreatedAt,
    DateTime? UpdatedAt
);
