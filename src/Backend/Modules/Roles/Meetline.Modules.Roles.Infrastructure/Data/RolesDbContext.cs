using Meetline.Modules.Roles.Application.Data;
using Meetline.Modules.Roles.Domain.Entities;
using Meetline.Modules.SharedKernel.Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Meetline.Modules.Roles.Infrastructure.Data;

public sealed class RolesDbContext(DbContextOptions options) : AuditingDbContext(options), IRolesDbContext
{
    public DbSet<Role> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RolesDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}