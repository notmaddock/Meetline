using Application.Common.Caching;
using Application.Common.Caching.Keys;
using FluentResults;
using Mediator;

namespace Application.Features.User.GetUsernameAvailability;

public record GetUsernameAvailabilityQuery : IQuery<Result<GetUsernameAvailabilityResponse>>, ICachableRequest
{
    public required string Username { get; init; }

    public string CacheKey => UserCacheKeys.UsernameAvailability(Username);
    public TimeSpan? AbsoluteExpiration => TimeSpan.FromMinutes(30);
    public TimeSpan? SlidingExpiration => TimeSpan.FromMinutes(10);
}