using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TaskManagement.Backend.Core.Validators;
using TaskManagement.Backend.Features.Tasks.Entities;
using TaskStatus = TaskManagement.Backend.Features.Tasks.Entities.TaskStatus;

namespace TaskManagement.Backend.Features.Tasks.Dtos;

public record TaskUpdateRequestDto(
    [Required(AllowEmptyStrings = false, ErrorMessage = "Title is required.")]
    [StringLength(
        250,
        MinimumLength = 5,
        ErrorMessage = "Title must be between 5 and 250 characters long."
    )]
        string Title,
    [NotBlank(ErrorMessage = "Description cannot be empty.")]
    [StringLength(
        5000,
        MinimumLength = 10,
        ErrorMessage = "Description must be between 10 and 5000 characters long."
    )]
        string? Description,
    [Required(ErrorMessage = "Status is required.")] [property: BindRequired] TaskStatus Status,
    [Required(ErrorMessage = "Priority is required.")]
    [property: BindRequired]
        TaskPriority Priority,
    [Required(ErrorMessage = "DueDate is required.")]
    [property: BindRequired]
    [NotPastDate(ErrorMessage = "DueDate must be a future date.", IsTodayAllowed = false)]
        DateOnly DueDate
);
