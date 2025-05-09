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

    [Column("created_at"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at"), DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime UpdatedAt { get; set; }

    [InverseProperty(nameof(TaskEntity.Project))]
    public IList<TaskEntity> Tasks { get; } = [];
}
