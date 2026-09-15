using FCG.Catalog.Domain.Services.Caching;

namespace CommonTestUtilities.Services;

public class FakeCacheService : ICacheService
{
    private readonly Dictionary<string, object> _memory = new();

    public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        if (_memory.TryGetValue(key, out var val) && val is T typed)
            return Task.FromResult<T?>(typed);

        return Task.FromResult<T?>(default);
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        if (value != null)
            _memory[key] = value;

        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        _memory.Remove(key);
        return Task.CompletedTask;
    }

    public Task RemoveByPrefixAsync(string prefixKey, CancellationToken cancellationToken = default)
    {
        var keysToRemove = _memory.Keys.Where(k => k.StartsWith(prefixKey)).ToList();
        foreach (var k in keysToRemove)
        {
            _memory.Remove(k);
        }

        return Task.CompletedTask;
    }

    public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        var cached = await GetAsync<T>(key, cancellationToken);
        if (cached != null)
            return cached;

        var result = await factory();
        if (result != null)
        {
            await SetAsync(key, result, expiration, cancellationToken);
        }

        return result;
    }
}

public class CacheServiceBuilder
{
    public static ICacheService Build()
    {
        return new FakeCacheService();
    }
}
