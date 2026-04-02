using Application.Scopes;
using Domain.Abstractions;
using Domain.Entities;
using Infrastructure.Abstractions;
using Infrastructure.EntityDefinitions;
using Infrastructure.EntityDefinitions.Postgres;
using Microsoft.EntityFrameworkCore;
using Tenant = Domain.Entities.Tenant;

namespace Infrastructure.Data;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    ICurrentUserScope? currentUserScope = null)
    : AuditingDbContext(options)
{
    public Guid CurrentTenantId => currentUserScope?.TenantId ?? Guid.Empty;

    public DbSet<User> Users => Set<User>();

    public DbSet<Role> Roles => Set<Role>();

    public DbSet<Tenant> Tenants => Set<Tenant>();

    public override int SaveChanges()
    {
        ApplyTenantIds();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyTenantIds();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyTenantIds()
    {
        var entries = ChangeTracker.Entries<ITenantOwned>();

        foreach (var entry in entries)
            if (entry.State == EntityState.Added && entry.Entity.TenantId == Guid.Empty)
                entry.Entity.TenantId = CurrentTenantId;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.MapRoleDefinitions();
        modelBuilder.ApplyTenantFilters(this);
    }
}