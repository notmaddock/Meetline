using Application.Common.Caching;
using Application.Common.Caching.Keys;
using Domain.Wrappers;
using FluentResults;
using Mediator;

namespace Application.Features.Role.CreateRole;

public record CreateRoleCommand(string Name, bool Hoist, int Position, PermissionSet Permissions)
    : ICommand<Result<Guid>>, IInvalidateCacheRequest
{
    public string[] CacheKeysToInvalidate =>
    [
        RoleCacheKeys.All,
        "roles:last_created" // TODO remove this as it is just to test cache invalidation
    ];
}