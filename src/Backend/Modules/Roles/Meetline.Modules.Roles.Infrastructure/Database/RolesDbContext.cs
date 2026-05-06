using Meetline.Modules.Roles.Domain.Entities;
using Meetline.Modules.SharedKernel.Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Meetline.Modules.Roles.Infrastructure.Database;

internal sealed class RolesDbContext(DbContextOptions options) : AuditingDbContext(options)
{
    internal DbSet<Role> Roles { get; set; }
}