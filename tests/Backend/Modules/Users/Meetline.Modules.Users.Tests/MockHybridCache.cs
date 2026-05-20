using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Hybrid;

namespace Meetline.Modules.Users.Tests;

public class MockHybridCache : HybridCache
{
    private readonly ConcurrentDictionary<string, object> _cache = new();

    public override ValueTask<T> GetOrCreateAsync<TState, T>(
        string key,
        TState state,
        Func<TState, CancellationToken, ValueTask<T>> factory,
        HybridCacheEntryOptions? options = null,
        IEnumerable<string>? tags = null,
        CancellationToken cancellationToken = default)
    {
        if (_cache.TryGetValue(key, out var val) && val is T typedVal)
        {
            return new ValueTask<T>(typedVal);
        }

        var task = factory(state, cancellationToken);
        if (task.IsCompletedSuccessfully)
        {
            var res = task.Result;
            _cache[key] = res!;
            return new ValueTask<T>(res);
        }

        return new ValueTask<T>(task.AsTask().ContinueWith(t =>
        {
            _cache[key] = t.Result!;
            return t.Result;
        }, cancellationToken));
    }

    public override ValueTask RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        _cache.TryRemove(key, out _);
        return ValueTask.CompletedTask;
    }

    public override ValueTask RemoveByTagAsync(string tag, CancellationToken cancellationToken = default)
    {
        return ValueTask.CompletedTask; // simplified
    }

    public override ValueTask SetAsync<T>(string key, T value, HybridCacheEntryOptions? options = null, IEnumerable<string>? tags = null, CancellationToken cancellationToken = default)
    {
        _cache[key] = value!;
        return ValueTask.CompletedTask;
    }
}
