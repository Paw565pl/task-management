using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TaskManagement.Backend.Core.Validators;
using TaskManagement.Backend.Features.Tasks.Entities;
using TaskStatus = TaskManagement.Backend.Features.Tasks.Entities.TaskStatus;

namespace TaskManagement.Backend.Features.Tasks.Dtos;

public record TaskUpdateRequestDto(
    [Required] [NotBlank] [StringLength(250, MinimumLength = 5)] string Title,
    [NotBlank] [StringLength(5000, MinimumLength = 10)] string? Description,
    [Required] [property: BindRequired] TaskStatus Status,
    [Required] [property: BindRequired] TaskPriority Priority,
    [Required] [property: BindRequired] DateOnly DueDate
);
