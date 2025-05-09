using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Backend.Features.Project.Entity;

namespace TaskManagement.Backend.Features.Task.Entity;

[Table("tasks"), Index(nameof(CreatedAt)), Index(nameof(UpdatedAt))]
public class TaskEntity
{
    public long Id { get; set; }

    [Column(TypeName = "varchar(200)")]
    public required string Title { get; set; }

    [Column(TypeName = "varchar(2000)")]
    public string? Description { get; set; }

    public TaskStatus TaskStatus { get; set; } = TaskStatus.Todo;

    public required TaskPriority TaskPriority { get; set; }

    public required DateOnly DueDate { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime? CreatedAt { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime? UpdatedAt { get; set; }

    public long ProjectId { get; set; }
    public required ProjectEntity Project { get; set; }
}
