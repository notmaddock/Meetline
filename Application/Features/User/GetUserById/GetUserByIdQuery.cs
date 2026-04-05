using Application.Common.Caching;
using Application.Common.Caching.Keys;
using Application.Features.User.DTOs.UserPublicResponse;
using FluentResults;
using Mediator;

namespace Application.Features.User.GetUserById;

/// <summary>
///     Gets a user by their ID, used for public queries (does not include private info)
/// </summary>
/// <param name="Id">The user's ID</param>
public record GetUserByIdQuery(Guid Id) : IQuery<Result<UserPublicResponse>>, ICachableRequest
{
    public string CacheKey => UserCacheKeys.ById(Id);
    public TimeSpan? AbsoluteExpiration => TimeSpan.FromMinutes(60);
    public TimeSpan? SlidingExpiration => TimeSpan.FromMinutes(5);
}