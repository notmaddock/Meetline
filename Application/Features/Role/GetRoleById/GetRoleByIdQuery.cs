using Application.Common.Caching;
using Application.Common.Caching.Keys;
using Application.Features.Role.DTOs.RoleResponse;
using FluentResults;
using Mediator;

namespace Application.Features.Role.GetRoleById;

public record GetRoleByIdQuery(Guid Id) : IQuery<Result<RoleResponse>>, ICachableRequest
{
    public string CacheKey => RoleCacheKeys.ById(Id);
    public TimeSpan? AbsoluteExpiration => TimeSpan.FromMinutes(10);
    public TimeSpan? SlidingExpiration => TimeSpan.FromMinutes(2);
}