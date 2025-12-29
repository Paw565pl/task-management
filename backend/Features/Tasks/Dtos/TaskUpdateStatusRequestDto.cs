using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TaskStatus = TaskManagement.Backend.Features.Tasks.Entities.TaskStatus;

namespace TaskManagement.Backend.Features.Tasks.Dtos;

public record TaskUpdateStatusRequestDto(
    [Required(ErrorMessage = "Status is required.")]
    [EnumDataType(typeof(TaskStatus), ErrorMessage = "Invalid status value.")]
    [property: BindRequired]
        TaskStatus Status
);
