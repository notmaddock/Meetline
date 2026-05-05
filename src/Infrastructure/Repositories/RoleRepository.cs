using Application.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RoleRepository(ApplicationDbContext context) : IRoleRepository
{
    public Task<List<Role>> GetRolesAsync(CancellationToken ct)
    {
        return context.Roles.AsNoTracking().ToListAsync(ct);
    }

    public Task<Role?> GetRoleByIdAsync(Guid id, CancellationToken ct)
    {
        return context.Roles.FirstOrDefaultAsync(r => r.Id == id, ct);
    }

    public async Task CreateRoleAsync(Role role, CancellationToken ct)
    {
        await context.Roles.AddAsync(role, ct);
        await context.SaveChangesAsync(ct);
    }
}