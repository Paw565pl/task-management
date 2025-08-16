using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Backend.Core.Dto;
using TaskManagement.Backend.Features.Project.Dto;
using TaskManagement.Backend.Features.Project.Service;

namespace TaskManagement.Backend.Features.Project.Controller;

[ApiController, Route("/api/v1/projects"), Authorize]
public class ProjectController(ProjectService projectService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PageResponseDto<ProjectResponseDto>>> GetAll(
        [FromQuery] SortOptionsDto? sortOptionsDto,
        [FromQuery] PageOptionsDto? pageOptionsDto,
        CancellationToken cancellationToken
    )
    {
        return Ok(await projectService.GetAllAsync(sortOptionsDto, pageOptionsDto, cancellationToken));
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<ProjectResponseDto>> GetById([FromRoute] long id, CancellationToken cancellationToken)
    {
        return Ok(await projectService.GetByIdAsync(id, cancellationToken));
    }

    [HttpPost]
    public async Task<ActionResult<ProjectResponseDto>> Create([FromBody] ProjectRequestDto projectRequestDto, CancellationToken cancellationToken)
    {
        var newProject = await projectService.CreateAsync(projectRequestDto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = newProject.Id }, newProject);
    }
}
