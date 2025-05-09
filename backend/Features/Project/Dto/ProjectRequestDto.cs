using System.ComponentModel.DataAnnotations;
using TaskManagement.Backend.Core.Validators;

namespace TaskManagement.Backend.Features.Project.Dto;

public record ProjectRequestDto(
    [NotBlank, StringLength(200, MinimumLength = 5)] string Name,
    [StringLength(2000, MinimumLength = 10)] string? Description);
