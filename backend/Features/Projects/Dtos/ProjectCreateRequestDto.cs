using System.ComponentModel.DataAnnotations;
using TaskManagement.Backend.Core.Validators;

namespace TaskManagement.Backend.Features.Projects.Dtos;

public record ProjectCreateRequestDto(
    [Required] [NotBlank] [StringLength(250, MinimumLength = 5)] string Name,
    [StringLength(5000, MinimumLength = 10)] string? Description
);
