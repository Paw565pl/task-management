using Microsoft.EntityFrameworkCore;
using TaskManagement.Backend.Features.Project.Entity;
using TaskManagement.Backend.Features.Task.Entity;

namespace TaskManagement.Backend.Core.Context;

public class AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) : DbContext(dbContextOptions)
{
    public DbSet<ProjectEntity> Projects => Set<ProjectEntity>();
    public DbSet<TaskEntity> Tasks => Set<TaskEntity>();
}
