namespace TaskManagement.Backend.Core.Db;

public interface IAuditableEntity
{
    public DateTime CreatedAt { get; }

    public DateTime UpdatedAt { get; }
}
