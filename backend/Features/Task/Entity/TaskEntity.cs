using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagement.Backend.Features.Project.Entity;

namespace TaskManagement.Backend.Features.Task.Entity;

[Table("tasks")]
[Index(nameof(Status))]
[Index(nameof(Priority))]
[Index(nameof(DueDate))]
[Index(nameof(CreatedAt))]
[Index(nameof(UpdatedAt))]
[EntityTypeConfiguration(typeof(TaskEntityConfiguration))]
public class TaskEntity
{
    [Key][Column("id")] public long Id { get; set; }

    [MaxLength(250)][Column("title")] public required string Title { get; set; }

    [MaxLength(5000)]
    [Column("description")]
    public string? Description { get; set; }

    [Column("status")] public TaskStatus Status { get; set; } = TaskStatus.Todo;

    [Column("priority")] public required TaskPriority Priority { get; set; }

    [Column("due_date")] public required DateOnly DueDate { get; set; }

    [Column("created_at")] public DateTime CreatedAt { get; private init; }

    [Column("updated_at")] public DateTime UpdatedAt { get; private set; }

    [ForeignKey(nameof(Project))]
    [Column("project_id")]
    public long ProjectId { get; init; }

    [InverseProperty(nameof(ProjectEntity.Tasks))]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public required ProjectEntity? Project { get; init; }

    public void RefreshUpdatedAt() => UpdatedAt = DateTime.UtcNow;
}

internal class TaskEntityConfiguration : IEntityTypeConfiguration<TaskEntity>
{
    public void Configure(EntityTypeBuilder<TaskEntity> builder)
    {
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP").ValueGeneratedOnAdd();
        builder.Property(x => x.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP").ValueGeneratedOnAdd(
            );
    }
}
