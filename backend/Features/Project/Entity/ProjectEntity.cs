using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Backend.Features.Task.Entity;

namespace TaskManagement.Backend.Features.Project.Entity;

[Table("projects"), Index(nameof(Name), IsUnique = true), Index(nameof(CreatedAt)), Index(nameof(UpdatedAt))]
public class ProjectEntity
{
    public long Id { get; set; }

    [Column(TypeName = "varchar(200)")]
    public required string Name { get; set; }

    [Column(TypeName = "varchar(2000)")]
    public string? Description { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime? CreatedAt { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime? UpdatedAt { get; set; }

    public Collection<TaskEntity> Tasks { get; } = [];
}
