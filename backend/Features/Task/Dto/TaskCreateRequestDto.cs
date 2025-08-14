using System.ComponentModel.DataAnnotations;
using TaskManagement.Backend.Core.Validators;
using TaskManagement.Backend.Features.Task.Entity;

namespace TaskManagement.Backend.Features.Task.Dto;

public record TaskCreateRequestDto(
    [NotBlank, StringLength(250, MinimumLength = 5)]
    string Title,
    [StringLength(5000, MinimumLength = 10)]
    string? Description,
    [Required] TaskPriority Priority,
    [Required] DateOnly DueDate
);
