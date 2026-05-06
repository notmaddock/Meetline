using Application.Features.Role.DTOs.RoleResponse;

namespace Application.Features.Role.GetRoleById;

public record GetRoleByIdQuery(Guid Id) : IQuery<Result<RoleResponse>>, ICacheableRequest
{
    public string CacheKey => RoleCacheKeys.ById(Id);
    public TimeSpan? AbsoluteExpiration => TimeSpan.FromMinutes(10);
    public TimeSpan? SlidingExpiration => TimeSpan.FromMinutes(2);
}