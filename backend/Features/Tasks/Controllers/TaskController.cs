using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Backend.Core.Dtos;
using TaskManagement.Backend.Features.Tasks.Dtos;
using TaskManagement.Backend.Features.Tasks.Services;

namespace TaskManagement.Backend.Features.Tasks.Controllers;

[Authorize]
[ApiController]
[Route("/api/v1/projects/{projectId:long}/tasks")]
public class TaskController(TaskService taskService) : ControllerBase
{
    [ProducesResponseType<PageResponseDto<TaskResponseDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationFailureResponseDto>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [HttpGet]
    public async Task<ActionResult<PageResponseDto<TaskResponseDto>>> GetAll(
        [FromRoute] long projectId,
        [FromQuery] TaskFiltersDto? taskFilterDto,
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

    [ProducesResponseType<TaskResponseDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [HttpGet("{taskId:long}")]
    public async Task<ActionResult<TaskResponseDto>> GetById(
        [FromRoute] long projectId,
        [FromRoute] long taskId,
        CancellationToken cancellationToken
    )
    {
        return Ok(await taskService.GetByIdAsync(projectId, taskId, cancellationToken));
    }

    [ProducesResponseType<TaskResponseDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationFailureResponseDto>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
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

    [ProducesResponseType<TaskResponseDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationFailureResponseDto>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
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

    [ProducesResponseType<TaskResponseDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationFailureResponseDto>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
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

    [ProducesResponseType<TaskResponseDto>(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
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
