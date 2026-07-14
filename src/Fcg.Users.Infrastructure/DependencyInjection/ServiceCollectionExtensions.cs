using Fcg.Users.Application.Security;
using Fcg.Users.Application.Events;
using Fcg.Users.Domain.Users;
using Fcg.Users.Infrastructure.Events;
using Fcg.Users.Infrastructure.Persistence;
using Fcg.Users.Infrastructure.Persistence.Repositories;
using Fcg.Users.Infrastructure.Security;
using Fcg.Users.Infrastructure.Messaging;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fcg.Users.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var provider = configuration["Database:Provider"] ?? "Sqlite";
        var connectionString = configuration["Database:ConnectionString"]
            ?? configuration.GetConnectionString("UsersDb");
        var jwtSettings = BuildJwtTokenSettings(configuration);

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Database connection string is required.");
        }

        services.AddDbContext<UsersDbContext>(options =>
        {
            switch (provider.Trim().ToLowerInvariant())
            {
                case "sqlite":
                    options.UseSqlite(connectionString);
                    break;
                default:
                    throw new InvalidOperationException(
                        $"Database provider '{provider}' is not supported in this MVP.");
            }
        });

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPasswordHasher, Pbkdf2PasswordHasher>();
        services.AddEventPublisher(configuration);
        services.AddSingleton(jwtSettings);
        services.AddSingleton<IAccessTokenGenerator, JwtAccessTokenGenerator>();

        return services;
    }

    private static IServiceCollection AddEventPublisher(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var rabbitMqOptions = RabbitMqOptions.FromConfiguration(configuration);

        if (!rabbitMqOptions.Enabled)
        {
            services.AddScoped<IEventPublisher, LoggingEventPublisher>();

            return services;
        }

        services.AddScoped<IEventPublisher, MassTransitEventPublisher>();
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.UsingRabbitMq((_, rabbitMqConfigurator) =>
            {
                rabbitMqConfigurator.UseRawJsonSerializer(
                    RawSerializerOptions.AddTransportHeaders | RawSerializerOptions.CopyHeaders,
                    isDefault: true);

                rabbitMqConfigurator.Host(
                    rabbitMqOptions.Host,
                    rabbitMqOptions.VirtualHost,
                    hostConfigurator =>
                    {
                        hostConfigurator.Username(rabbitMqOptions.Username);
                        hostConfigurator.Password(rabbitMqOptions.Password);
                    });

                rabbitMqConfigurator.Message<UserCreatedEvent>(messageConfigurator =>
                    messageConfigurator.SetEntityName(EventNames.UserCreated));
            });
        });

        return services;
    }

    private static JwtTokenSettings BuildJwtTokenSettings(IConfiguration configuration)
    {
        var settings = new JwtTokenSettings
        {
            Issuer = configuration["Jwt:Issuer"] ?? string.Empty,
            Audience = configuration["Jwt:Audience"] ?? string.Empty,
            SecretKey = configuration["Jwt:SecretKey"] ?? string.Empty,
            ExpirationInMinutes = int.TryParse(configuration["Jwt:ExpirationInMinutes"], out var expirationInMinutes)
                ? expirationInMinutes
                : 60
        };

        if (string.IsNullOrWhiteSpace(settings.Issuer) ||
            string.IsNullOrWhiteSpace(settings.Audience) ||
            string.IsNullOrWhiteSpace(settings.SecretKey))
        {
            throw new InvalidOperationException("JWT settings are required.");
        }

        return settings;
    }
}
