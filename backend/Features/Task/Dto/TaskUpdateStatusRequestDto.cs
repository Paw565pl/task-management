using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Backend.Features.Task.Dto;

public record TaskUpdateStatusRequestDto(
    [Required] TaskStatus TaskStatus);
