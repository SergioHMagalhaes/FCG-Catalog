using System.Text.Json;
using System.Text.Json.Serialization;
using FCG.Catalog.Domain.Services.Caching;
using FCG.Catalog.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace FCG.Catalog.Infrastructure.Services.Caching;

public class RedisCacheService : ICacheService
{
    private readonly IConnectionMultiplexer? _redis;
    private readonly RedisSettings _settings;
    private readonly ILogger<RedisCacheService> _logger;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public RedisCacheService(
        IOptions<RedisSettings> settings,
        ILogger<RedisCacheService> logger,
        IConnectionMultiplexer? redis = null)
    {
        _settings = settings.Value;
        _logger = logger;
        _redis = redis;
    }

    private IDatabase? GetDatabase()
    {
        if (_redis == null || !_redis.IsConnected)
            return null;

        try
        {
            return _redis.GetDatabase();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to retrieve Redis database.");
            return null;
        }
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            var db = GetDatabase();
            if (db == null)
                return default;

            var value = await db.StringGetAsync(key);
            if (value.IsNullOrEmpty)
                return default;

            return JsonSerializer.Deserialize<T>((string)value!, JsonOptions);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to get key '{Key}' from Redis cache. Falling back to source.", key);
            return default;
        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var db = GetDatabase();
            if (db == null)
                return;

            var ttl = expiration ?? TimeSpan.FromMinutes(_settings.DefaultTtlMinutes);
            var serialized = JsonSerializer.Serialize(value, JsonOptions);

            await db.StringSetAsync(key, serialized, ttl);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to set key '{Key}' in Redis cache.", key);
        }
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            var db = GetDatabase();
            if (db == null)
                return;

            await db.KeyDeleteAsync(key);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to remove key '{Key}' from Redis cache.", key);
        }
    }

    public async Task RemoveByPrefixAsync(string prefixKey, CancellationToken cancellationToken = default)
    {
        try
        {
            if (_redis == null || !_redis.IsConnected)
                return;

            var db = _redis.GetDatabase();
            var endpoints = _redis.GetEndPoints();

            foreach (var endpoint in endpoints)
            {
                var server = _redis.GetServer(endpoint);
                if (!server.IsConnected)
                    continue;

                var pattern = $"{prefixKey}*";
                var keys = new List<RedisKey>();

                await foreach (var key in server.KeysAsync(pattern: pattern))
                {
                    keys.Add(key);
                }

                if (keys.Count > 0)
                {
                    await db.KeyDeleteAsync(keys.ToArray());
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to remove keys with prefix '{Prefix}' from Redis cache.", prefixKey);
        }
    }

    public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        var cached = await GetAsync<T>(key, cancellationToken);
        if (cached != null)
        {
            return cached;
        }

        var result = await factory();

        if (result != null)
        {
            await SetAsync(key, result, expiration, cancellationToken);
        }

        return result!;
    }
}
