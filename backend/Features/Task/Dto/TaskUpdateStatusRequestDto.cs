using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TaskStatus = TaskManagement.Backend.Features.Task.Entity.TaskStatus;

namespace TaskManagement.Backend.Features.Task.Dto;

public record TaskUpdateStatusRequestDto([Required] [property: BindRequired] TaskStatus Status);
