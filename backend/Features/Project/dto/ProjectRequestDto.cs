using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Backend.Features.Project.dto;

public record ProjectRequestDto(
    [Required] string Name,
    string? Description);
