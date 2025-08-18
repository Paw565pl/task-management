namespace TaskManagement.Backend.Core.Dtos;

public record SortOptionsDto(string? SortBy, SortDirection SortDirection = SortDirection.Asc);
