using Riok.Mapperly.Abstractions;
using TaskManagement.Backend.Features.Task.Dto;
using TaskManagement.Backend.Features.Task.Entity;

namespace TaskManagement.Backend.Features.Task.Mapper;

[Mapper]
public static partial class TaskMapper
{
    [MapperIgnoreTarget(nameof(TaskEntity.Id))]
    [MapperIgnoreTarget(nameof(TaskEntity.ProjectId))]
    [MapperIgnoreTarget(nameof(TaskEntity.Project))]
    [MapperIgnoreTarget(nameof(TaskEntity.CreatedAt))]
    [MapperIgnoreTarget(nameof(TaskEntity.UpdatedAt))]
    [MapperIgnoreTarget(nameof(TaskEntity.Status))]
    public static partial TaskEntity ToEntity(TaskCreateRequestDto taskCreateRequestDto);

    [MapperIgnoreTarget(nameof(TaskEntity.Id))]
    [MapperIgnoreTarget(nameof(TaskEntity.ProjectId))]
    [MapperIgnoreTarget(nameof(TaskEntity.Project))]
    [MapperIgnoreTarget(nameof(TaskEntity.CreatedAt))]
    [MapperIgnoreTarget(nameof(TaskEntity.UpdatedAt))]
    public static partial TaskEntity ToEntity(TaskUpdateRequestDto taskCreateRequestDto);

    [MapperIgnoreSource(nameof(TaskEntity.ProjectId))]
    [MapperIgnoreSource(nameof(TaskEntity.Project))]
    public static partial TaskResponseDto ToResponseDto(TaskEntity taskEntity);

    public static partial IQueryable<TaskResponseDto> ToResponseDto(this IQueryable<TaskEntity> taskEntities);
}
