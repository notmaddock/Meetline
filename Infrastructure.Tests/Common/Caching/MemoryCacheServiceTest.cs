using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Common.Caching;
using JetBrains.Annotations;
using Microsoft.Extensions.Caching.Memory;
using Xunit;

namespace Infrastructure.Tests.Common.Caching;

[TestSubject(typeof(MemoryCacheService))]
public class MemoryCacheServiceTest
{
    private readonly MemoryCacheService _service;

    public MemoryCacheServiceTest()
    {
        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        _service = new MemoryCacheService(memoryCache);
    }

    [Fact(DisplayName = "Should return the cached value when the key exists")]
    public async Task GetOrCreateAsync_WhenValueExists_ShouldReturnCachedValue()
    {
        const string key = "test-key";
        const string value = "cached-value";

        _service.Set(key, value);

        var result = await _service.GetOrCreateAsync(key, _ => Task.FromResult("new-value"),
            ct: TestContext.Current.CancellationToken);

        Assert.Equal(value, result); // (shouldn't be "new-value" since the factory shouldn't run)
    }

    [Fact(DisplayName = "Should call the factory and populate the cache when the key does not exist")]
    public async Task GetOrCreateAsync_WhenValueDoesNotExist_ShouldCallFactory()
    {
        const string key = "test-key";
        const string expectedValue = "new-value";

        var result = await _service.GetOrCreateAsync(key, _ => Task.FromResult(expectedValue),
            ct: TestContext.Current.CancellationToken);

        Assert.Equal(expectedValue, result);
        Assert.Equal(expectedValue, _service.Get<string>(key));
    }

    [Fact(DisplayName = "Should ensure atomic execution so the factory is only called once during concurrent access")]
    public async Task GetOrCreateAsync_WhenMultipleCallsAreConcurrent_ShouldBeAtomic()
    {
        const string key = "atomic-key";
        var callCount = 0;

        var tasks = Enumerable.Range(0, 10)
            .Select(_ => _service.GetOrCreateAsync(key, Factory, ct: TestContext.Current.CancellationToken))
            .ToList();

        var results = await Task.WhenAll(tasks);

        Assert.All(results, r => Assert.Equal("populated-value", r));
        Assert.Equal(1, callCount);

        return;

        async Task<string> Factory(CancellationToken ct)
        {
            Interlocked.Increment(ref callCount);
            await Task.Delay(100, ct);
            return "populated-value";
        }
    }
}