using Microsoft.AspNetCore.Mvc;
using TaskManagement.Backend.Core.Dto;
using TaskManagement.Backend.Features.Task.Dto;
using TaskManagement.Backend.Features.Task.Service;

namespace TaskManagement.Backend.Features.Task.Controller;

[ApiController, Route("/ap1/v1/projects/{projectId:long}/tasks")]
public class TaskController(TaskService taskService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PageResponseDto<TaskResponseDto>>> GetAll([FromRoute] long projectId,
        [FromQuery] SortOptionsDto? sortOptionsDto, [FromQuery] PageOptionsDto? pageOptionsDto)
    {
        return Ok(await taskService.GetAllAsync(projectId, sortOptionsDto, pageOptionsDto));
    }

    [HttpGet("{taskId:long}")]
    public async Task<ActionResult<TaskResponseDto>> GetById([FromRoute] long projectId, [FromRoute] long taskId)
    {
        return Ok(await taskService.GetByIdAsync(projectId, taskId));
    }

    [HttpPost]
    public async Task<ActionResult<TaskResponseDto>> Create([FromRoute] long projectId,
        [FromBody] TaskCreateRequestDto taskCreateRequestDto)
    {
        var newTask = await taskService.CreateAsync(projectId, taskCreateRequestDto);
        return CreatedAtAction(nameof(GetById), new { projectId, taskId = newTask.Id }, newTask);
    }

    [HttpPut("{taskId:long}")]
    public async Task<ActionResult<TaskResponseDto>> Update([FromRoute] long projectId, [FromRoute] long taskId,
        [FromBody] TaskUpdateRequestDto taskUpdateRequestDto)
    {
        return Ok(await taskService.UpdateAsync(projectId, taskId, taskUpdateRequestDto));
    }

    [HttpPatch("{taskId:long}")]
    public async Task<ActionResult<TaskResponseDto>> UpdateStatus([FromRoute] long projectId, [FromRoute] long taskId,
        [FromBody] TaskUpdateStatusRequestDto taskUpdateStatusRequestDto)
    {
        return Ok(await taskService.UpdateStatusAsync(projectId, taskId, taskUpdateStatusRequestDto));
    }

    [HttpDelete("{taskId:long}")]
    public async Task<ActionResult> Delete([FromRoute] long projectId, [FromRoute] long taskId)
    {
        await taskService.DeleteAsync(projectId, taskId);
        return NoContent();
    }
}
