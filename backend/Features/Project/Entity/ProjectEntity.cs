using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagement.Backend.Features.Task.Entity;

namespace TaskManagement.Backend.Features.Project.Entity;

[Table("projects")]
[Index(nameof(Name), Name = UniqueNameConstraint, IsUnique = true)]
[Index(nameof(CreatedAt))]
[Index(nameof(UpdatedAt))]
[EntityTypeConfiguration(typeof(ProjectEntityConfiguration))]
public class ProjectEntity
{
    public const string UniqueNameConstraint = "IX_projects_name";

    [Key][Column("id")] public long Id { get; set; }

    [MaxLength(250)][Column("name")] public required string Name { get; set; }

    [MaxLength(5000)]
    [Column("description")]
    public string? Description { get; set; }

    [Column("created_at")] public DateTime CreatedAt { get; private init; }

    [Column("updated_at")] public DateTime UpdatedAt { get; private set; }

    [InverseProperty(nameof(TaskEntity.Project))]
    public ICollection<TaskEntity> Tasks { get; } = [];

    public void RefreshUpdatedAt() => UpdatedAt = DateTime.UtcNow;
}

internal class ProjectEntityConfiguration : IEntityTypeConfiguration<ProjectEntity>
{
    public void Configure(EntityTypeBuilder<ProjectEntity> builder)
    {
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP").ValueGeneratedOnAdd();
        builder.Property(x => x.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP").ValueGeneratedOnAdd();
    }
}
