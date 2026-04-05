using Application.Common.Caching;
using Application.Common.Caching.Keys;
using Application.Features.User.DTOs.UserResponse;
using FluentResults;
using Mediator;

namespace Application.Features.User.GetOwnUserById;

/// <summary>
///     Gets a user from their ID, assuming in a private context. This query might return information considered private
/// </summary>
/// <param name="Id"></param>
public record GetOwnUserByIdQuery(Guid Id) : IQuery<Result<UserResponse>>, ICachableRequest
{
    public string CacheKey => UserCacheKeys.ById(Id);
    public TimeSpan? AbsoluteExpiration => TimeSpan.FromMinutes(30);
    public TimeSpan? SlidingExpiration => TimeSpan.FromMinutes(15);
}