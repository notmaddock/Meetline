using System.Reflection;
using Meetline.Modules.Permissions.Contracts.Metadata;

namespace Domain.Registries;

public static class PermissionRegistry
{
    private static readonly PermissionMetadata[] ByIndex;

    /// <summary>
    ///     Same as having all permissions, additionally allows bypassing specific non-global overrides.
    /// </summary>
    public static readonly PermissionMetadata Administrator = new(0, PermissionScope.Global);

    /// <summary>
    ///     Allows user management, including full edit access, creation and deletion.
    /// </summary>
    public static readonly PermissionMetadata ManageUsers = new(1, PermissionScope.Global);

    /// <summary>
    ///     Allows role management, including full edit access, creation and deletion. Some restrictions apply to prevent
    ///     privilege escalation.
    /// </summary>
    public static readonly PermissionMetadata ManageRoles = new(2, PermissionScope.Global);

    /// <summary>
    ///     Allows room management, including full edit access, creation and deletion.
    /// </summary>
    public static readonly PermissionMetadata ManageRooms = new(3, PermissionScope.Global);

    static PermissionRegistry()
    {
        var fields = typeof(PermissionRegistry)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => f.FieldType == typeof(PermissionMetadata));

        var permissions = fields
            .Select(f => (PermissionMetadata)f.GetValue(null)!)
            .ToArray();

        var maxIndex = permissions.Max(p => p.BitIndex);

        ByIndex = new PermissionMetadata[maxIndex + 1];

        var expectedIndex = 0;

        foreach (var permission in permissions)
        {
            if (permission.BitIndex != expectedIndex++)
                throw new ArgumentException("Permission has a non-contiguous bit index and will waste bitfield space");

            ByIndex[permission.BitIndex] = permission;
        }
    }

    public static PermissionMetadata FromIndex(int index)
    {
        return ByIndex[index];
    }
}