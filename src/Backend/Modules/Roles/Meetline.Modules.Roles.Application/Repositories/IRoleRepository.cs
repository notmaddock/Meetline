using Meetline.Modules.Roles.Domain.Entities;

namespace Meetline.Modules.Roles.Application.Repositories;

public interface IRoleRepository
{
    Task<List<Role>> GetRolesAsync(CancellationToken ct);
    Task<Role?> GetRoleByIdAsync(Guid id, CancellationToken ct);
    Task CreateRoleAsync(Role role, CancellationToken ct);
}