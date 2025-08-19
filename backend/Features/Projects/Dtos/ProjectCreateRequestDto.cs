using System.ComponentModel.DataAnnotations;
using TaskManagement.Backend.Core.Validators;

namespace TaskManagement.Backend.Features.Projects.Dtos;

public record ProjectCreateRequestDto(
    [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required.")]
    [StringLength(
        250,
        MinimumLength = 5,
        ErrorMessage = "Name must be between 5 and 250 characters long."
    )]
        string Name,
    [NotBlank(ErrorMessage = "Description cannot be empty.")]
    [StringLength(
        5000,
        MinimumLength = 10,
        ErrorMessage = "Description must be between 10 and 5000 characters long."
    )]
        string? Description
);
