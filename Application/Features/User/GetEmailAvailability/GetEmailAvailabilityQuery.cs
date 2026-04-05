using Application.Common.Caching;
using Application.Common.Caching.Keys;
using FluentResults;
using Mediator;

namespace Application.Features.User.GetEmailAvailability;

public record GetEmailAvailabilityQuery : IQuery<Result<GetEmailAvailabilityResponse>>, ICachableRequest
{
    public required string Email { get; init; }

    public string CacheKey => UserCacheKeys.EmailAvailability(Email);
    public TimeSpan? AbsoluteExpiration => TimeSpan.FromMinutes(30);
    public TimeSpan? SlidingExpiration => TimeSpan.FromMinutes(10);
}