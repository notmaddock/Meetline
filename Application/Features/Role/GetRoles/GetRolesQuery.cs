using Application.Common.Caching;
using Application.Common.Caching.Keys;
using Application.Features.Role.DTOs.RoleResponse;
using FluentResults;
using Mediator;

namespace Application.Features.Role.GetRoles;

public record GetRolesQuery : IQuery<Result<ICollection<RoleResponse>>>, ICachableRequest
{
    public string CacheKey => RoleCacheKeys.All;
    public TimeSpan? AbsoluteExpiration => TimeSpan.FromMinutes(10);
    public TimeSpan? SlidingExpiration => TimeSpan.FromMinutes(2);
}