using Meetline.Modules.Roles.Application.Repositories;
using Meetline.Modules.Roles.Domain.Entities;
using Meetline.Modules.Roles.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Meetline.Modules.Roles.Infrastructure.Repositories;

internal class RoleRepository(RolesDbContext ctx) : IRoleRepository
{
    public Task<List<Role>> GetRolesAsync(CancellationToken ct)
    {
        return ctx.Roles.AsNoTracking().ToListAsync(ct);
    }

    public Task<Role?> GetRoleByIdAsync(Guid id, CancellationToken ct)
    {
        return ctx.Roles.FirstOrDefaultAsync(r => r.Id == id, ct);
    }

    public async Task CreateRoleAsync(Role role, CancellationToken ct)
    {
        await ctx.Roles.AddAsync(role, ct);
        await ctx.SaveChangesAsync(ct);
    }
}