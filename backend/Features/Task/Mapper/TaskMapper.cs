using Riok.Mapperly.Abstractions;
using TaskManagement.Backend.Features.Task.Dto;
using TaskManagement.Backend.Features.Task.Entity;

namespace TaskManagement.Backend.Features.Task.Mapper;

[Mapper]
public partial class TaskMapper
{
    [MapperIgnoreTarget(nameof(TaskEntity.Project))]
    public partial TaskEntity ToEntity(TaskCreateRequestDto taskCreateRequestDto);

    [MapperIgnoreTarget(nameof(TaskEntity.Project))]
    public partial TaskEntity ToEntity(TaskUpdateRequestDto taskCreateRequestDto);

    public partial TaskResponseDto ToResponseDto(TaskEntity taskEntity);
}
