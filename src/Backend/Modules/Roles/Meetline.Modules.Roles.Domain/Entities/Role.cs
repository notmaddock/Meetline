using Meetline.Modules.SharedKernel.Domain.Abstractions;
using Meetline.Modules.SharedKernel.Domain.Wrappers;

namespace Meetline.Modules.Roles.Domain.Entities;

public class Role : AuditableEntity
{
    public Guid Id { get; init; }

    /// <summary>
    ///     The role name
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    ///     Whether the role is hoisted. Hoisted roles show separately in user lists and separate groups of users. This is
    ///     handled only on the frontend and is irrelevant to the server.
    /// </summary>
    public bool Hoist { get; set; }

    /// <summary>
    ///     The role position. Higher positions increase precedence, mostly relevant in overrides. The role with maximum
    ///     position determines the user's overall position, which in turn determines who can act upon said user.
    /// </summary>
    public int Position { get; set; }

    /// <summary>
    ///     A permission set the role grants. All PermissionSets will be OR'ed to calculate the user's "total" permissions.
    /// </summary>
    public required PermissionSet Permissions { get; init; }
}