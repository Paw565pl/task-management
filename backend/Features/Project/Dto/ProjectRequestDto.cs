using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Backend.Features.Project.Dto;

public record ProjectRequestDto(
    [Required] string Name,
    string? Description);
