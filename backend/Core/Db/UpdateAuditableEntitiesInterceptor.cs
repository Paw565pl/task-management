using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace TaskManagement.Backend.Core.Db;

public class UpdateAuditableEntitiesInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default
    )
    {
        var changeTracker = eventData.Context?.ChangeTracker;
        if (changeTracker is null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        var utcNow = DateTime.UtcNow;
        var entities = changeTracker.Entries<IAuditableEntity>();

        foreach (var entity in entities)
        {
            if (entity.State == EntityState.Added)
            {
                entity.Property(nameof(IAuditableEntity.CreatedAt)).CurrentValue = utcNow;
                entity.Property(nameof(IAuditableEntity.UpdatedAt)).CurrentValue = utcNow;
            }

            if (entity.State == EntityState.Modified)
            {
                entity.Property(nameof(IAuditableEntity.UpdatedAt)).CurrentValue = utcNow;
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
