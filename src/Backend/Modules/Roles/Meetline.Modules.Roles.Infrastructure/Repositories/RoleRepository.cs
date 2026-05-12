using Meetline.Modules.Roles.Application.Data;
using Meetline.Modules.Roles.Application.Repositories;

namespace Meetline.Modules.Roles.Infrastructure.Repositories;

internal class RoleRepository(IRolesDbContext ctx) : IRoleRepository
{
}