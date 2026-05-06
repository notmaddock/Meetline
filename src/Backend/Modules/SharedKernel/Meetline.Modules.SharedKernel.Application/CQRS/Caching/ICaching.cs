namespace Meetline.Modules.SharedKernel.Application.CQRS.Caching;

public interface ICacheableRequest
{
    string CacheKey { get; }

    /// <summary>
    ///     The fixed time after which the cache entry MUST expire.
    /// </summary>
    TimeSpan? AbsoluteExpiration { get; }

    /// <summary>
    ///     The rolling time that refreshes every time the key is accessed.
    /// </summary>
    TimeSpan? SlidingExpiration { get; }
}

public interface ICacheInvalidatingRequest
{
    string[] CacheKeysToInvalidate { get; }
}