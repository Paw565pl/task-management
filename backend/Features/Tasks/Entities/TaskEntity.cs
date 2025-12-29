using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Backend.Core.Db;
using TaskManagement.Backend.Features.Projects.Entities;

namespace TaskManagement.Backend.Features.Tasks.Entities;

[Table("tasks")]
[Index(nameof(Status))]
[Index(nameof(Priority))]
[Index(nameof(DueDate))]
[Index(nameof(CreatedAt))]
[Index(nameof(UpdatedAt))]
public class TaskEntity : IAuditableEntity
{
    [Key]
    [Column("id")]
    public long Id { get; private init; }

    [MaxLength(250)]
    [Column("title")]
    public required string Title { get; set; }

    [MaxLength(5000)]
    [Column("description")]
    public string? Description { get; set; }

    [Column("status")]
    public TaskStatus Status { get; set; } = TaskStatus.Todo;

    [Column("priority")]
    public required TaskPriority Priority { get; set; }

    [Column("due_date")]
    public required DateOnly DueDate { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; private init; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; private init; }

    [ForeignKey(nameof(Project))]
    [Column("project_id")]
    public long ProjectId { get; init; }

    [InverseProperty(nameof(ProjectEntity.Tasks))]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public required ProjectEntity? Project { get; init; }
}
