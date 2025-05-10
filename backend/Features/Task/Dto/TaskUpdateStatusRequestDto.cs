using System.ComponentModel.DataAnnotations;
using TaskStatus = TaskManagement.Backend.Features.Task.Entity.TaskStatus;

namespace TaskManagement.Backend.Features.Task.Dto;

public record TaskUpdateStatusRequestDto(
    [Required] TaskStatus Status);
