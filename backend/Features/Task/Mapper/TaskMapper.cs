using Riok.Mapperly.Abstractions;
using TaskManagement.Backend.Features.Task.Dto;
using TaskManagement.Backend.Features.Task.Entity;

namespace TaskManagement.Backend.Features.Task.Mapper;

[Mapper]
public static partial class TaskMapper
{
    [MapValue(nameof(TaskEntity.Project), null)]
    public static partial TaskEntity ToEntity(TaskCreateRequestDto taskCreateRequestDto);

    [MapValue(nameof(TaskEntity.Project), null)]
    public static partial TaskEntity ToEntity(TaskUpdateRequestDto taskCreateRequestDto);

    public static partial TaskResponseDto ToResponseDto(TaskEntity taskEntity);

    public static partial IQueryable<TaskResponseDto> ToResponseDto(this IQueryable<TaskEntity> taskEntities);
}
