using FluentResults;
using Mediator;
using Meetline.Modules.SharedKernel.Application.CQRS.Caching;
using Meetline.Modules.SharedKernel.Application.CQRS.Caching.Keys;

namespace Meetline.Modules.Users.Application.Users.Queries.Exists;

/// <summary>
///     Checks if a user exists given their ID
/// </summary>
/// <param name="Id">The user's ID</param>
public record ExistsQuery(Guid Id) : IQuery<Result<ExistsResponse>>, ICacheableRequest
{
    public string CacheKey => UserCacheKeys.Exists(Id);
    public TimeSpan? AbsoluteExpiration => TimeSpan.FromMinutes(30);
    public TimeSpan? SlidingExpiration => TimeSpan.FromMinutes(5);
}