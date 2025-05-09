using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Backend.Features.Project.Entity;

namespace TaskManagement.Backend.Features.Task.Entity;

[Table("tasks"), Index(nameof(CreatedAt)), Index(nameof(UpdatedAt))]
public class TaskEntity
{
    [Key, Column("id")] public long Id { get; set; }

    [Column("title"), MaxLength(200)] public required string Title { get; set; }

    [Column("description"), MaxLength(2000)]
    public string? Description { get; set; }

    [Column("status")] public TaskStatus Status { get; set; } = TaskStatus.Todo;

    [Column("priority")] public required TaskPriority Priority { get; set; }

    [Column("due_date")] public required DateOnly DueDate { get; set; }

    [Column("created_at")] public DateTime CreatedAt { get; } = DateTime.UtcNow;

    [Column("updated_at")] public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [Column("project_id"), ForeignKey(nameof(Project))]
    public long ProjectId { get; set; }

    [InverseProperty(nameof(ProjectEntity.Tasks)), DeleteBehavior(DeleteBehavior.Cascade)]
    public required ProjectEntity Project { get; set; }
}
