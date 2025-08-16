using System.ComponentModel.DataAnnotations;
using TaskManagement.Backend.Core.Validators;

namespace TaskManagement.Backend.Features.Project.Dto;

public record ProjectCreateRequestDto(
    [NotBlank, StringLength(250, MinimumLength = 5)]
    string Name,
    [StringLength(5000, MinimumLength = 10)]
    string? Description);
