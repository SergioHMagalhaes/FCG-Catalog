using FCG.Catalog.Domain.Messaging;
using FCG.Catalog.Domain.Repositories;
using FCG.Catalog.Domain.Services.LoggedUser;
using FCG.Catalog.Infrastructure.DataAccess.Document;
using FCG.Catalog.Infrastructure.DataAccess.Document.Repositories;
using FCG.Catalog.Infrastructure.DataAccess.Relational;
using FCG.Catalog.Infrastructure.DataAccess.Relational.Repositories;
using FCG.Catalog.Infrastructure.Messaging;
using FCG.Catalog.Infrastructure.Services.LoggedUser;
using FCG.Catalog.Infrastructure.Settings;
using FCG.Infrastructure.Settings;
using FCG.Shared.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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
}
