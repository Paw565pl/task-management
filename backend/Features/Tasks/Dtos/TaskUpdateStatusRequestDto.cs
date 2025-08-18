using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TaskStatus = TaskManagement.Backend.Features.Tasks.Entities.TaskStatus;

namespace TaskManagement.Backend.Features.Tasks.Dtos;

public record TaskUpdateStatusRequestDto([Required] [property: BindRequired] TaskStatus Status);
