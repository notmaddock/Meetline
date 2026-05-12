using Meetline.Modules.SharedKernel.Infrastructure.Abstractions;
using Meetline.Modules.Users.Application.Data;
using Meetline.Modules.Users.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Meetline.Modules.Users.Infrastructure.Data;

public sealed class UsersDbContext(DbContextOptions options) : AuditingDbContext(options), IUsersDbContext
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UsersDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}