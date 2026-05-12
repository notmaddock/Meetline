using System.Collections;
using Meetline.Modules.Roles.Domain.Entities;
using Meetline.Modules.SharedKernel.Domain.Wrappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Meetline.Modules.Roles.Infrastructure.Entities;

public class RoleEntityConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        var permissionSetConverter = new ValueConverter<PermissionSet, BitArray>(
            ps => ps.ToBitArray(),
            ba => PermissionSet.FromBitArray(ba)
        );

        var permissionSetComparer = new ValueComparer<PermissionSet>(
            (l, r) => l.Equals(r),
            s => s.GetHashCode(),
            s => s);

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Permissions)
            .HasConversion(permissionSetConverter, permissionSetComparer)
            .HasColumnType("VARBIT");
    }
}