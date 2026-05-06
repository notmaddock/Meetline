namespace Meetline.Modules.Permissions.Contracts.Metadata;

/// <summary>
///     Defines the scopes the permission works in. This controls where the permission may be used and if it can be
///     overriden.
///     Notice this is a bitfield enum, meaning a permission can have multiple scopes (or none, which wouldn't be of much
///     use)
/// </summary>
public enum PermissionScope
{
    /// <summary>
    ///     The permission is global, meaning it can apply in a global scope (added to a role, for example).
    /// </summary>
    Global,

    /// <summary>
    ///     The permission applies to a room, meaning it can be overriden
    /// </summary>
    Room
}

public record PermissionMetadata(int BitIndex, PermissionScope Scope)
{
    public static implicit operator int(PermissionMetadata metadata)
    {
        return metadata.BitIndex;
    }
}