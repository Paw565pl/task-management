using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Backend.Features.Task.Entity;

namespace TaskManagement.Backend.Features.Project.Entity;

[Table("projects"), Index(nameof(Name), IsUnique = true), Index(nameof(CreatedAt)), Index(nameof(UpdatedAt))]
public class ProjectEntity
{
    [Key, Column("id")] public long Id { get; set; }

    [Column("name"), MaxLength(200)] public required string Name { get; set; }

    [Column("description"), MaxLength(2000)]
    public string? Description { get; set; }

    [Column("created_at")] public DateTime CreatedAt { get; } = DateTime.UtcNow;

    [Column("updated_at")] public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [InverseProperty(nameof(TaskEntity.Project))]
    public IList<TaskEntity> Tasks { get; } = [];
}
