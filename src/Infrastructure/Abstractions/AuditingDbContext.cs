using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Abstractions;

public abstract class AuditingDbContext(DbContextOptions options) : DbContext(options)
{
    public override int SaveChanges()
    {
        ApplyAuditFields();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        ApplyAuditFields();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyAuditFields()
    {
        var entries = ChangeTracker
            .Entries<AuditableEntity>();

        var now = DateTime.UtcNow;

        foreach (var entry in entries)
            // ReSharper disable once ConvertIfStatementToSwitchStatement
            if (entry.State == EntityState.Added)
                entry.Entity.CreatedAt = now;
            else if (entry.State == EntityState.Modified) entry.Entity.UpdatedAt = now;
    }
}