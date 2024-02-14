using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SharedKernel;

namespace Persistence.Interceptors;

internal sealed class UpdateDeletableInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            UpdateDeletableEntities(eventData.Context);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void UpdateDeletableEntities(DbContext context)
    {
        DateTime utcNow = DateTime.UtcNow;
        var entities = context.ChangeTracker.Entries<IDeletable>().ToList();

        foreach (EntityEntry<IDeletable> entry in entities)
        {
            if (entry.State != EntityState.Deleted) 
                continue;

            entry.Property(nameof(IDeletable.DeletedOnUtc)).CurrentValue = utcNow;

            entry.Property(nameof(IDeletable.Deleted)).CurrentValue = true;

            entry.State = EntityState.Modified;
            
            UpdateDeletedEntityEntryReferencesToUnchanged(entry);
        }
    }
    
    private static void UpdateDeletedEntityEntryReferencesToUnchanged(EntityEntry entityEntry)
    {
        if (!entityEntry.References.Any())
            return;

        foreach (var referenceEntry in entityEntry.References.Where(r => r.TargetEntry?.State == EntityState.Deleted))
        {
            if (referenceEntry.TargetEntry == null) 
                continue;
            
            referenceEntry.TargetEntry.State = EntityState.Unchanged;

            UpdateDeletedEntityEntryReferencesToUnchanged(referenceEntry.TargetEntry);
        }
    }
}