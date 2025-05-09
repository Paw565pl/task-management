using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Backend.Features.Project.Dto;
using TaskManagement.Backend.Features.Project.Service;

namespace TaskManagement.Backend.Features.Project.Controller;

[ApiController, Route("/api/v1/projects")]
public class ProjectController(ProjectService projectService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await projectService.GetAllAsync());
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById([FromRoute] long id)
    {
        var project = await projectService.GetByIdAsync(id);
        if (project is null) return NotFound();

        return Ok(project);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProjectRequestDto projectRequestDto)
    {
        try
        {
            var newProject = await projectService.CreateAsync(projectRequestDto);
            return CreatedAtAction(nameof(GetById), new { id = newProject.Id }, newProject);
        }
        catch (DbUpdateException)
        {
            var problem = new ProblemDetails
            {
                Title = "Integrity error.",
                Detail = "Name must be unique.",
            };
            return Conflict(problem);
        }
    }
}
