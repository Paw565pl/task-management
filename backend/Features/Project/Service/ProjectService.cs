using Microsoft.EntityFrameworkCore;
using TaskManagement.Backend.Core.Context;
using TaskManagement.Backend.Features.Project.Dto;
using TaskManagement.Backend.Features.Project.Mapper;
using TaskStatus = TaskManagement.Backend.Features.Task.Entity.TaskStatus;

namespace TaskManagement.Backend.Features.Project.Service;

public class ProjectService(AppDbContext appDbContext, ProjectMapper projectMapper)
{
    public async Task<IList<ProjectResponseDto>> GetAllAsync()
    {
        return await appDbContext.ProjectEntities.Select(project => new ProjectResponseDto
            (
                project.Id,
                project.Name,
                project.Description,
                project.Tasks.Count,
                project.Tasks.Count(task => task.Status == TaskStatus.Done),
                project.CreatedAt,
                project.UpdatedAt
            )).AsNoTracking()
            .ToListAsync();
    }

    public async Task<ProjectResponseDto?> GetByIdAsync(long id)
    {
        return await appDbContext.ProjectEntities.Where(project => project.Id == id).Select(project =>
            new ProjectResponseDto
            (
                project.Id,
                project.Name,
                project.Description,
                project.Tasks.Count,
                project.Tasks.Count(task => task.Status == TaskStatus.Done),
                project.CreatedAt,
                project.UpdatedAt
            )).AsNoTracking().FirstOrDefaultAsync();
    }

    public async Task<ProjectResponseDto> CreateAsync(ProjectRequestDto projectRequestDto)
    {
        var project = projectMapper.ToEntity(projectRequestDto);
        var savedProject = await appDbContext.ProjectEntities.AddAsync(project);
        await appDbContext.SaveChangesAsync();

        return projectMapper.ToResponseDto(savedProject.Entity);
    }
}
