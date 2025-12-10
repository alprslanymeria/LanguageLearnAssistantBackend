using App.Application.Contracts.Infrastructure;
using App.Domain.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace App.Infrastructure.Caching;

public static class RedisCachingExtension
{
    public static IServiceCollection AddRedisCaching(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        var redisOption = configuration.GetSection(RedisOption.Key).Get<RedisOption>()
                         ?? throw new InvalidOperationException("Redis configuration section is missing or invalid.");

        services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(redisOption.ConnectionString));
        services.AddSingleton<ICacheService>(sp =>
        {
            var connectionMultiplexer = sp.GetRequiredService<IConnectionMultiplexer>();
            return new RedisCacheService(connectionMultiplexer, redisOption.Database);
        });

        return services;
    }
}
