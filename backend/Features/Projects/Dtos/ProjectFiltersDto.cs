using TaskManagement.Backend.Core.Validators;

namespace TaskManagement.Backend.Features.Projects.Dtos;

public record ProjectFiltersDto(
    [NotBlank(ErrorMessage = "SearchTerm cannot be empty.")] string? SearchTerm
);
