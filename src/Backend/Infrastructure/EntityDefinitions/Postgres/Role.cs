using System.Collections;
using Domain.Entities;
using Domain.Entities.Defaults;
using Domain.Wrappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.EntityDefinitions.Postgres;

internal static class RoleDefinitions
{
    internal static void MapRoleDefinitions(this ModelBuilder modelBuilder)
    {
        var permissionSetConverter = new ValueConverter<PermissionSet, BitArray>(
            ps => ps.ToBitArray(),
            ba => PermissionSet.FromBitArray(ba)
        );

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(r => r.Id);

            entity.Property(r => r.Permissions)
                .HasConversion(permissionSetConverter)
                .HasColumnType("VARBIT");

            entity.HasData(DefaultRoles.EveryoneRole);
        });
    }
}