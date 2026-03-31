namespace Application.Common.Caching;

public interface ICacheService
{
    T? Get<T>(string key);
    void Set<T>(string key, T value, TimeSpan? absoluteExpiration = null, TimeSpan? slidingExpiration = null);
    void Remove(string key);

    /// <summary>
    ///     Atomic Get or Create. Ensures only one caller populates the cache for a given key.
    /// </summary>
    Task<T> GetOrCreateAsync<T>(string key, Func<CancellationToken, Task<T>> factory,
        TimeSpan? absoluteExpiration = null, TimeSpan? slidingExpiration = null, CancellationToken ct = default);
}