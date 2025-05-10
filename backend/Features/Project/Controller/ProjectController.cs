using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Backend.Core.Dto;
using TaskManagement.Backend.Features.Project.Dto;
using TaskManagement.Backend.Features.Project.Service;

namespace TaskManagement.Backend.Features.Project.Controller;

[ApiController, Route("/api/v1/projects")]
public class ProjectController(ProjectService projectService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PageResponseDto<ProjectResponseDto>>> GetAll(
        [FromQuery] SortOptionsDto? sortOptionsDto,
        [FromQuery] PageOptionsDto? pageOptionsDto
    )
    {
        return Ok(await projectService.GetAllAsync(sortOptionsDto, pageOptionsDto));
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<ProjectResponseDto>> GetById([FromRoute] long id)
    {
        return Ok(await projectService.GetByIdAsync(id));
    }

    [HttpPost]
    public async Task<ActionResult<ProjectResponseDto>> Create([FromBody] ProjectRequestDto projectRequestDto)
    {
        var newProject = await projectService.CreateAsync(projectRequestDto);
        return CreatedAtAction(nameof(GetById), new { id = newProject.Id }, newProject);
    }
}
