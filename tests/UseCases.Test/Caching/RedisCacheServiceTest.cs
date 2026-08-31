using FCG.Catalog.Domain.Constants;
using FCG.Catalog.Infrastructure.Services.Caching;
using FCG.Catalog.Infrastructure.Settings;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace UseCases.Test.Caching;

public class RedisCacheServiceTest
{
    [Fact]
    public async Task GetAsync_WhenRedisNotConnected_ShouldReturnDefaultSafely()
    {
        var settings = Options.Create(new RedisSettings());
        var logger = NullLogger<RedisCacheService>.Instance;
        var service = new RedisCacheService(settings, logger, redis: null);

        var result = await service.GetAsync<string>("test-key");

        Assert.Null(result);
    }

    [Fact]
    public async Task SetAsync_WhenRedisNotConnected_ShouldNotThrowException()
    {
        var settings = Options.Create(new RedisSettings());
        var logger = NullLogger<RedisCacheService>.Instance;
        var service = new RedisCacheService(settings, logger, redis: null);

        var exception = await Record.ExceptionAsync(() => service.SetAsync("test-key", "value"));

        Assert.Null(exception);
    }

    [Fact]
    public async Task GetOrCreateAsync_WhenRedisNotConnected_ShouldExecuteFactoryAndReturnValueSafely()
    {
        var settings = Options.Create(new RedisSettings
        {
            ConnectionString = "localhost:6379",
            DefaultTtlMinutes = 15
        });
        var logger = NullLogger<RedisCacheService>.Instance;
        var service = new RedisCacheService(settings, logger, redis: null);

        var executed = false;
        var result = await service.GetOrCreateAsync("test-key", () =>
        {
            executed = true;
            return Task.FromResult("hello-world");
        });

        Assert.True(executed);
        Assert.Equal("hello-world", result);
    }

    [Fact]
    public async Task RemoveAsync_WhenRedisNotConnected_ShouldNotThrowException()
    {
        var settings = Options.Create(new RedisSettings());
        var logger = NullLogger<RedisCacheService>.Instance;
        var service = new RedisCacheService(settings, logger, redis: null);

        var exception = await Record.ExceptionAsync(() => service.RemoveAsync("test-key"));

        Assert.Null(exception);
    }

    [Fact]
    public async Task RemoveByPrefixAsync_WhenRedisNotConnected_ShouldNotThrowException()
    {
        var settings = Options.Create(new RedisSettings());
        var logger = NullLogger<RedisCacheService>.Instance;
        var service = new RedisCacheService(settings, logger, redis: null);

        var exception = await Record.ExceptionAsync(() => service.RemoveByPrefixAsync("fcg:catalog:games:"));

        Assert.Null(exception);
    }

    [Fact]
    public void CacheKeys_Games_List_ShouldGenerateDeterministicKey()
    {
        var key1 = CacheKeys.Games.List(1, 10, 1, false, "Action");
        var key2 = CacheKeys.Games.List(1, 10, 1, false, "Action");
        var key3 = CacheKeys.Games.List(1, 10, 1, false, "RPG");

        Assert.StartsWith(CacheKeys.Games.ListPrefix, key1);
        Assert.Equal(key1, key2);
        Assert.NotEqual(key1, key3);
    }

    [Fact]
    public void CacheKeys_Categories_ShouldFormatCorrectly()
    {
        Assert.Equal("fcg:catalog:categories:all", CacheKeys.Categories.All);
        Assert.Equal("fcg:catalog:categories:id:42", CacheKeys.Categories.ById(42));
    }
}
