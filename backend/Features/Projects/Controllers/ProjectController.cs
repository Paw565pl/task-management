using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Backend.Core.Dtos;
using TaskManagement.Backend.Features.Projects.Dtos;
using TaskManagement.Backend.Features.Projects.Services;

namespace TaskManagement.Backend.Features.Projects.Controllers;

[Authorize]
[ApiController]
[Route("/api/v1/projects")]
public class ProjectController(ProjectService projectService) : ControllerBase
{
    [ProducesResponseType<PageResponseDto<ProjectResponseDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationFailureResponseDto>(StatusCodes.Status400BadRequest)]
    [HttpGet]
    public async Task<ActionResult<PageResponseDto<ProjectResponseDto>>> GetAll(
        [FromQuery] ProjectFiltersDto? projectFiltersDto,
        [FromQuery] SortOptionsDto? sortOptionsDto,
        [FromQuery] PageOptionsDto? pageOptionsDto,
        CancellationToken cancellationToken
    )
    {
        return Ok(
            await projectService.GetAllAsync(
                projectFiltersDto,
                sortOptionsDto,
                pageOptionsDto,
                cancellationToken
            )
        );
    }

    [ProducesResponseType<ProjectResponseDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [HttpGet("{id:long}")]
    public async Task<ActionResult<ProjectResponseDto>> GetById(
        [FromRoute] long id,
        CancellationToken cancellationToken
    )
    {
        return Ok(await projectService.GetByIdAsync(id, cancellationToken));
    }

    [ProducesResponseType<ProjectResponseDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationFailureResponseDto>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    [HttpPost]
    public async Task<ActionResult<ProjectResponseDto>> Create(
        [FromBody] ProjectCreateRequestDto projectCreateRequestDto,
        CancellationToken cancellationToken
    )
    {
        var newProject = await projectService.CreateAsync(
            projectCreateRequestDto,
            cancellationToken
        );
        return CreatedAtAction(nameof(GetById), new { id = newProject.Id }, newProject);
    }
}
