using Application.Common.Caching;
using Application.Common.Caching.Keys;
using Application.Features.User.DTOs.UserGuidResponse;
using FluentResults;
using Mediator;

namespace Application.Features.User.GetUserIdByExternalId;

/// <summary>
///     A query for getting a user's internal ID from their external ID
/// </summary>
/// <param name="ExternalId">The user's external ID</param>
public record GetUserIdByExternalIdQuery(string ExternalId)
    : IQuery<Result<UserGuidResponse>>, ICachableRequest
{
    public string CacheKey => UserCacheKeys.ByExternalId(ExternalId);
    public TimeSpan? AbsoluteExpiration => null;
    public TimeSpan? SlidingExpiration => TimeSpan.FromMinutes(30);
}