using System.ComponentModel.DataAnnotations;
using TaskManagement.Backend.Core.Validators;
using TaskManagement.Backend.Features.Task.Entity;
using TaskStatus = TaskManagement.Backend.Features.Task.Entity.TaskStatus;

namespace TaskManagement.Backend.Features.Task.Dto;

public record TaskUpdateRequestDto(
    [NotBlank, StringLength(200, MinimumLength = 5)] string Title,
    [StringLength(2000, MinimumLength = 10)] string? Description,
    [Required] TaskStatus TaskStatus,
    [Required] TaskPriority TaskPriority,
    [Required] DateOnly DueDate);
