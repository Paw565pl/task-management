using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Backend.Core.Dtos;
using TaskManagement.Backend.Features.Tasks.Dtos;
using TaskManagement.Backend.Features.Tasks.Service;

namespace TaskManagement.Backend.Features.Tasks.Controllers;

[ApiController, Route("/api/v1/projects/{projectId:long}/tasks"), Authorize]
public class TaskController(TaskService taskService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PageResponseDto<TaskResponseDto>>> GetAll(
        [FromRoute] long projectId,
        [FromQuery] TaskFilterDto? taskFilterDto,
        [FromQuery] SortOptionsDto? sortOptionsDto,
        [FromQuery] PageOptionsDto? pageOptionsDto,
        CancellationToken cancellationToken
    )
    {
        return Ok(
            await taskService.GetAllAsync(
                projectId,
                taskFilterDto,
                sortOptionsDto,
                pageOptionsDto,
                cancellationToken
            )
        );
    }

    [HttpGet("{taskId:long}")]
    public async Task<ActionResult<TaskResponseDto>> GetById(
        [FromRoute] long projectId,
        [FromRoute] long taskId,
        CancellationToken cancellationToken
    )
    {
        return Ok(await taskService.GetByIdAsync(projectId, taskId, cancellationToken));
    }

    [HttpPost]
    public async Task<ActionResult<TaskResponseDto>> Create(
        [FromRoute] long projectId,
        [FromBody] TaskCreateRequestDto taskCreateRequestDto,
        CancellationToken cancellationToken
    )
    {
        var newTask = await taskService.CreateAsync(
            projectId,
            taskCreateRequestDto,
            cancellationToken
        );
        return CreatedAtAction(nameof(GetById), new { projectId, taskId = newTask.Id }, newTask);
    }

    [HttpPut("{taskId:long}")]
    public async Task<ActionResult<TaskResponseDto>> Update(
        [FromRoute] long projectId,
        [FromRoute] long taskId,
        [FromBody] TaskUpdateRequestDto taskUpdateRequestDto,
        CancellationToken cancellationToken
    )
    {
        return Ok(
            await taskService.UpdateAsync(
                projectId,
                taskId,
                taskUpdateRequestDto,
                cancellationToken
            )
        );
    }

    [HttpPatch("{taskId:long}/status")]
    public async Task<ActionResult<TaskResponseDto>> UpdateStatus(
        [FromRoute] long projectId,
        [FromRoute] long taskId,
        [FromBody] TaskUpdateStatusRequestDto taskUpdateStatusRequestDto,
        CancellationToken cancellationToken
    )
    {
        return Ok(
            await taskService.UpdateStatusAsync(
                projectId,
                taskId,
                taskUpdateStatusRequestDto,
                cancellationToken
            )
        );
    }

    [HttpDelete("{taskId:long}")]
    public async Task<ActionResult> Delete(
        [FromRoute] long projectId,
        [FromRoute] long taskId,
        CancellationToken cancellationToken
    )
    {
        await taskService.DeleteAsync(projectId, taskId, cancellationToken);
        return NoContent();
    }
}
