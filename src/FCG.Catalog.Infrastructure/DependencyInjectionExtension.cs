using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using FCG.Catalog.Domain.Messaging;
using FCG.Catalog.Domain.Repositories;
using FCG.Catalog.Domain.Services.Caching;
using FCG.Catalog.Domain.Services.LoggedUser;
using FCG.Catalog.Domain.Services.Search;
using FCG.Catalog.Infrastructure.DataAccess.Document;
using FCG.Catalog.Infrastructure.DataAccess.Document.Repositories;
using FCG.Catalog.Infrastructure.DataAccess.Relational;
using FCG.Catalog.Infrastructure.DataAccess.Relational.Repositories;
using FCG.Catalog.Infrastructure.Messaging;
using FCG.Catalog.Infrastructure.Services.Caching;
using FCG.Catalog.Infrastructure.Services.LoggedUser;
using FCG.Catalog.Infrastructure.Services.Search;
using FCG.Catalog.Infrastructure.Settings;
using FCG.Infrastructure.Settings;
using FCG.Shared.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace FCG.Catalog.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ILoggedUser, LoggedUser>();

        AddDbContext(services, configuration);
        AddMongoDbContext(services, configuration);
        AddRepositories(services);
        AddMessaging(services, configuration);
        AddCaching(services, configuration);
        AddSearch(services, configuration);
    }

    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));
    }

    private static void AddMongoDbContext(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoSettings>(configuration.GetSection("MongoDB"));
        services.AddSingleton<MongoDbContext>();
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IGameRepository, GameRepository>();
        services.AddScoped<IGameOrderRepository, GameOrderRepository>();
        services.AddScoped<ILibraryRepository, LibraryRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
    }

    private static void AddMessaging(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqSettings>(
            configuration.GetSection(RabbitMqSettings.SectionName));

        services.Configure<JwtSettings>(
            configuration.GetSection(JwtSettings.SectionName));

        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                var rabbitMqSettings = context
                    .GetRequiredService<IOptions<RabbitMqSettings>>()
                    .Value; 

                if (rabbitMqSettings == null || 
                    string.IsNullOrWhiteSpace(rabbitMqSettings.Host) ||
                    string.IsNullOrWhiteSpace(rabbitMqSettings.Username) ||
                    string.IsNullOrWhiteSpace(rabbitMqSettings.Password))
                {
                    throw new InvalidOperationException("RabbitMQ settings are not configured.");
                }

                cfg.Host(
                    host: rabbitMqSettings.Host,
                    virtualHost: rabbitMqSettings.VirtualHost ?? "/",
                    h =>
                    {
                        h.Username(rabbitMqSettings.Username);
                        h.Password(rabbitMqSettings.Password);
                    });

                cfg.ConfigureEndpoints(context);
            });
        });

        services.AddScoped<IEventPublisher, EventPublisher>();
    }

    private static void AddCaching(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RedisSettings>(
            configuration.GetSection(RedisSettings.SectionName));

        var redisSettings = configuration
            .GetSection(RedisSettings.SectionName)
            .Get<RedisSettings>();

        if (!string.IsNullOrWhiteSpace(redisSettings?.ConnectionString))
        {
            try
            {
                var options = ConfigurationOptions.Parse(redisSettings.ConnectionString);
                options.AbortOnConnectFail = false;
                options.ConnectTimeout = 3000;
                options.AsyncTimeout = 3000;

                var multiplexer = ConnectionMultiplexer.Connect(options);
                services.AddSingleton<IConnectionMultiplexer>(multiplexer);
            }
            catch
            {
                // Resilient fallback: Redis will be null and RedisCacheService will gracefully bypass
            }
        }

        services.AddScoped<ICacheService, RedisCacheService>();
    }

    private static void AddSearch(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ElasticsearchSettings>(
            configuration.GetSection(ElasticsearchSettings.SectionName));

        var elasticsearchSettings = configuration
            .GetSection(ElasticsearchSettings.SectionName)
            .Get<ElasticsearchSettings>();

        if (!string.IsNullOrWhiteSpace(elasticsearchSettings?.Uri))
        {
            try
            {
                var clientSettings = new ElasticsearchClientSettings(new Uri(elasticsearchSettings.Uri))
                    .DefaultIndex(elasticsearchSettings.IndexName);

                if (!string.IsNullOrWhiteSpace(elasticsearchSettings.ApiKey))
                {
                    clientSettings = clientSettings.Authentication(new ApiKey(elasticsearchSettings.ApiKey));
                }
                else if (!string.IsNullOrWhiteSpace(elasticsearchSettings.Username) &&
                         !string.IsNullOrWhiteSpace(elasticsearchSettings.Password))
                {
                    clientSettings = clientSettings.Authentication(
                        new BasicAuthentication(elasticsearchSettings.Username, elasticsearchSettings.Password));
                }

                var client = new ElasticsearchClient(clientSettings);
                services.AddSingleton(client);
            }
            catch
            {
                // Resilient fallback: Elasticsearch client will be null and GameSearchService will gracefully bypass
            }
        }

        services.AddScoped<IGameSearchService, GameSearchService>();
    }
}

