namespace TaskManagement.Backend.Core.Dto;

public record SortOptionsDto(
    string? SortBy,
    SortDirection SortDirection = SortDirection.Asc
);
