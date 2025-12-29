using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Backend.Core.Dtos;

public record SortOptionsDto(
    string? SortBy,
    [EnumDataType(typeof(SortDirection), ErrorMessage = "Invalid sortDirection value.")]
        SortDirection SortDirection = SortDirection.Asc
);
