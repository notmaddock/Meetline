using Meetline.Modules.SharedKernel.Domain.Wrappers;

namespace Meetline.Modules.Roles.Domain.Entities.Defaults;

public static class DefaultRoles
{
    public static readonly Role EveryoneRole;

    static DefaultRoles()
    {
        EveryoneRole = new Role
        {
            Id = Guid.ParseExact("EEEEEEEE-EEEE-EEEE-EEEE-EEEEEEEEEEEE", "D"),
            Name = "Everyone",
            Hoist = false,
            Permissions = PermissionSet.None,
            Position = 0
        };
    }
}