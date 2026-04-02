using System.Reflection;
using Domain.Abstractions;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityDefinitions;

internal static class MultiTenancyExtensions
{
    private static readonly MethodInfo SetTenantOwnedFilterMethod = typeof(MultiTenancyExtensions)
        .GetMethod(nameof(SetTenantOwnedFilter), BindingFlags.NonPublic | BindingFlags.Static)!;

    internal static void ApplyTenantFilters(this ModelBuilder modelBuilder, ApplicationDbContext context)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            if (typeof(ITenantOwned).IsAssignableFrom(entityType.ClrType))
            {
                var method = SetTenantOwnedFilterMethod.MakeGenericMethod(entityType.ClrType);
                method.Invoke(null, [modelBuilder, context]);
            }
    }

    private static void SetTenantOwnedFilter<T>(ModelBuilder modelBuilder, ApplicationDbContext context)
        where T : class, ITenantOwned
    {
        modelBuilder.Entity<T>().HasQueryFilter(e => e.TenantId == context.CurrentTenantId);
    }
}