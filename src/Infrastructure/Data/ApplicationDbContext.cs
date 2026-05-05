using Domain.Entities;
using Infrastructure.Abstractions;
using Infrastructure.EntityDefinitions.Postgres;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : AuditingDbContext(options)
{
    public DbSet<User> Users => Set<User>();

    public DbSet<Role> Roles => Set<Role>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.MapUserDefinitions();
        modelBuilder.MapRoleDefinitions();
    }
}