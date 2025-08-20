using Microsoft.EntityFrameworkCore;
using TaskManagement.Backend.Features.Projects.Entities;
using TaskManagement.Backend.Features.Tasks.Entities;

namespace TaskManagement.Backend.Core.Db;

public class AppDbContext(DbContextOptions<AppDbContext> dbContextOptions)
    : DbContext(dbContextOptions)
{
    public DbSet<ProjectEntity> Projects => Set<ProjectEntity>();
    public DbSet<TaskEntity> Tasks => Set<TaskEntity>();
}
