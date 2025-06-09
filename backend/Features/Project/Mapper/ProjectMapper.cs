using Riok.Mapperly.Abstractions;
using TaskManagement.Backend.Features.Project.Dto;
using TaskManagement.Backend.Features.Project.Entity;
using TaskManagement.Backend.Features.Task.Entity;
using TaskStatus = TaskManagement.Backend.Features.Task.Entity.TaskStatus;

namespace TaskManagement.Backend.Features.Project.Mapper;

[Mapper]
public static partial class ProjectMapper
{
    [MapperIgnoreTarget(nameof(ProjectEntity.Id))]
    [MapperIgnoreTarget(nameof(ProjectEntity.UpdatedAt))]
    public static partial ProjectEntity ToEntity(ProjectRequestDto projectRequestDto);

    [MapProperty(nameof(ProjectEntity.Tasks), nameof(ProjectResponseDto.TaskCount), Use = nameof(MapTaskCount))]
    [MapProperty(nameof(ProjectEntity.Tasks), nameof(ProjectResponseDto.CompletedTaskCount),
        Use = nameof(MapCompletedTaskCount))]
    public static partial ProjectResponseDto ToResponseDto(ProjectEntity projectEntity);

    public static partial IQueryable<ProjectResponseDto> ToResponseDto(this IQueryable<ProjectEntity> projectEntities);

    private static int MapTaskCount(IList<TaskEntity> tasks) => tasks.Count;

    private static int MapCompletedTaskCount(IList<TaskEntity> tasks) => tasks.Count(task => task.Status == TaskStatus.Done);
}
