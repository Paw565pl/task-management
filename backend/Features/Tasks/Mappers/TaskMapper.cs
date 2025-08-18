using Riok.Mapperly.Abstractions;
using TaskManagement.Backend.Features.Projects.Entities;
using TaskManagement.Backend.Features.Tasks.Dtos;
using TaskManagement.Backend.Features.Tasks.Entities;

namespace TaskManagement.Backend.Features.Tasks.Mappers;

[Mapper]
public static partial class TaskMapper
{
    [MapperIgnoreTarget(nameof(TaskEntity.Id))]
    [MapperIgnoreTarget(nameof(TaskEntity.Status))]
    [MapperIgnoreTarget(nameof(TaskEntity.CreatedAt))]
    [MapperIgnoreTarget(nameof(TaskEntity.UpdatedAt))]
    [MapperIgnoreTarget(nameof(TaskEntity.ProjectId))]
    public static partial TaskEntity ToEntity(
        this TaskCreateRequestDto taskCreateRequestDto,
        ProjectEntity project
    );

    [MapperIgnoreTarget(nameof(TaskEntity.Id))]
    [MapperIgnoreTarget(nameof(TaskEntity.CreatedAt))]
    [MapperIgnoreTarget(nameof(TaskEntity.UpdatedAt))]
    [MapperIgnoreTarget(nameof(TaskEntity.ProjectId))]
    public static partial TaskEntity ToEntity(
        this TaskUpdateRequestDto taskCreateRequestDto,
        ProjectEntity project
    );

    [MapperIgnoreSource(nameof(TaskEntity.ProjectId))]
    [MapperIgnoreSource(nameof(TaskEntity.Project))]
    public static partial TaskResponseDto ToResponseDto(this TaskEntity taskEntity);

    public static partial IQueryable<TaskResponseDto> ToResponseDto(
        this IQueryable<TaskEntity> taskEntities
    );
}
