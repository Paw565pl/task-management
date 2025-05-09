using Riok.Mapperly.Abstractions;
using TaskManagement.Backend.Features.Project.Dto;
using TaskManagement.Backend.Features.Project.Entity;
using TaskManagement.Backend.Features.Task.Entity;
using TaskStatus = TaskManagement.Backend.Features.Task.Entity.TaskStatus;

namespace TaskManagement.Backend.Features.Project.Mapper;

[Mapper]
public partial class ProjectMapper
{
    public partial ProjectEntity ToEntity(ProjectRequestDto projectRequestDto);

    [MapProperty(nameof(ProjectEntity.Tasks), nameof(ProjectResponseDto.TaskCount), Use = nameof(MapTaskCount))]
    [MapProperty(nameof(ProjectEntity.Tasks), nameof(ProjectResponseDto.CompletedTaskCount),
        Use = nameof(MapCompletedTaskCount))]
    public partial ProjectResponseDto ToResponseDto(ProjectEntity projectEntity);

    protected int MapTaskCount(IList<TaskEntity> tasks) => tasks.Count;

    protected int MapCompletedTaskCount(IList<TaskEntity> tasks) => tasks.Count(task => task.Status == TaskStatus.Done);
}
