using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityDefinitions.Postgres;

internal static class UserDefinitions
{
    internal static void MapUserDefinitions(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(u => u.Id).HasDefaultValueSql("gen_random_uuid()");

            entity.HasIndex(u => u.ExternalId).IsUnique();
            entity.HasIndex(u => u.Username).IsUnique();
            entity.HasIndex(u => u.Email).IsUnique();
        });
    }
}