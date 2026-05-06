using Application.Features.Role.DTOs.CreateRoleRequest;
using Application.Features.Role.DTOs.RoleResponse;

namespace Application.Features.Role.CreateRole;

public record CreateRoleCommand(CreateRoleRequest Request)
    : ICommand<Result<RoleResponse>>, ICacheInvalidatingRequest
{
    public string[] CacheKeysToInvalidate =>
    [
        RoleCacheKeys.All
    ];
}