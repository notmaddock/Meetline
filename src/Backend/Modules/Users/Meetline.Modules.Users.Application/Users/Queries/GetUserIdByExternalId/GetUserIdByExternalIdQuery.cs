using FluentResults;
using Mediator;
using Meetline.Modules.SharedKernel.Application.CQRS.Caching;
using Meetline.Modules.SharedKernel.Application.CQRS.Caching.Keys;
using Meetline.Modules.Users.Application.Users.DTOs.UserGuidResponse;

namespace Meetline.Modules.Users.Application.Users.Queries.GetUserIdByExternalId;

/// <summary>
///     A query for getting a user's internal ID from their external ID
/// </summary>
/// <param name="ExternalId">The user's external ID</param>
public record GetUserIdByExternalIdQuery(string ExternalId)
    : IQuery<Result<UserGuidResponse>>, ICacheableRequest
{
    public string CacheKey => UserCacheKeys.ByExternalId(ExternalId);
    public TimeSpan? AbsoluteExpiration => null;
    public TimeSpan? SlidingExpiration => TimeSpan.FromMinutes(30);
}