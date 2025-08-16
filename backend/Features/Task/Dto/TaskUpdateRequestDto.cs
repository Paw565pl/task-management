using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TaskManagement.Backend.Core.Validators;
using TaskManagement.Backend.Features.Task.Entity;
using TaskStatus = TaskManagement.Backend.Features.Task.Entity.TaskStatus;

namespace TaskManagement.Backend.Features.Task.Dto;

public record TaskUpdateRequestDto(
    [Required]
    [NotBlank]
    [StringLength(250, MinimumLength = 5)]
    string Title,
    [NotBlank]
    [StringLength(5000, MinimumLength = 10)]
    string? Description,
    [Required][property: BindRequired] TaskStatus Status,
    [Required][property: BindRequired] TaskPriority Priority,
    [Required][property: BindRequired] DateOnly DueDate);
