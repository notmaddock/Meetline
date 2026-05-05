using Application.Common.Caching;
using FluentResults;
using Mediator;
using Microsoft.Extensions.Logging;

namespace Application.Common.PipelineBehaviors;

public sealed class CachingBehavior<TMessage, TResponse>(
    ICacheService cache,
    ILogger<CachingBehavior<TMessage, TResponse>> logger)
    : IPipelineBehavior<TMessage, TResponse>
    where TMessage : IMessage
{
    public async ValueTask<TResponse> Handle(TMessage message, MessageHandlerDelegate<TMessage, TResponse> next,
        CancellationToken cancellationToken)
    {
        #region Caching

        if (message is ICacheableRequest cachable)
            try
            {
                return await cache.GetOrCreateAsync(
                    cachable.CacheKey,
                    async ct => await next(message, ct),
                    cachable.AbsoluteExpiration,
                    cachable.SlidingExpiration,
                    cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Cache failure for key {CacheKey}. Falling back to handler.", cachable.CacheKey);
                return await next(message, cancellationToken);
            }

        #endregion

        var result = await next(message, cancellationToken);

        // Bail out if the message is not an invalidation request or the main result is a failure (which we don't want to cache)
        if (message is not ICacheInvalidatingRequest invalidator || result is not ResultBase { IsSuccess: true })
            return result;

        #region Invalidation

        foreach (var key in invalidator.CacheKeysToInvalidate)
            try
            {
                cache.Remove(key);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to invalidate cache key {CacheKey}", key);
            }

        #endregion

        return result;
    }
}