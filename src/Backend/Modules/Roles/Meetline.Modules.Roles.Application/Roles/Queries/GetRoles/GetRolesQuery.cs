using Application.Features.Role.DTOs.RoleResponse;

namespace Application.Features.Role.GetRoles;

public record GetRolesQuery : IQuery<Result<ICollection<RoleResponse>>>, ICacheableRequest
{
    public string CacheKey => RoleCacheKeys.All;
    public TimeSpan? AbsoluteExpiration => TimeSpan.FromMinutes(10);
    public TimeSpan? SlidingExpiration => TimeSpan.FromMinutes(2);
}