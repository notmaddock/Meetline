using System.Collections.Concurrent;
using Application.Common.Caching;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Common.Caching;

public sealed class MemoryCacheService(IMemoryCache cache) : ICacheService
{
    private static readonly ConcurrentDictionary<string, SemaphoreSlim> Locks = new();

    public T? Get<T>(string key)
    {
        return cache.Get<T>(key);
    }

    public void Set<T>(string key, T value, TimeSpan? absoluteExpiration = null, TimeSpan? slidingExpiration = null)
    {
        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = absoluteExpiration,
            SlidingExpiration = slidingExpiration
        };

        cache.Set(key, value, options);
    }

    public void Remove(string key)
    {
        cache.Remove(key);
    }

    public async Task<T> GetOrCreateAsync<T>(string key, Func<CancellationToken, Task<T>> factory,
        TimeSpan? absoluteExpiration = null, TimeSpan? slidingExpiration = null, CancellationToken ct = default)
    {
        if (cache.TryGetValue(key, out T? result) && result is not null) return result;

        var semaphore = Locks.GetOrAdd(key, _ => new SemaphoreSlim(1, 1));
        await semaphore.WaitAsync(ct);

        try
        {
            if (cache.TryGetValue(key, out result) && result is not null) return result;

            result = await factory(ct);

            Set(key, result, absoluteExpiration, slidingExpiration);
            return result;
        }
        finally
        {
            semaphore.Release();
        }
    }
}