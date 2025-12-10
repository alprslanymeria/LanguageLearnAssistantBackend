using System.Text.Json;
using App.Application.Contracts.Infrastructure;
using StackExchange.Redis;

namespace App.Infrastructure.Caching;

public class RedisCacheService : ICacheService
{
    private readonly IDatabase _database;

    public RedisCacheService(IConnectionMultiplexer connectionMultiplexer, int database = 0)
    {
        ArgumentNullException.ThrowIfNull(connectionMultiplexer);
        _database = connectionMultiplexer.GetDatabase(database);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(key);
        var payload = JsonSerializer.Serialize(value);
        await _database.StringSetAsync(key, payload, expiration).ConfigureAwait(false);
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(key);
        var result = await _database.StringGetAsync(key).ConfigureAwait(false);

        if (result.IsNullOrEmpty)
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(result!);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(key);
        await _database.KeyDeleteAsync(key).ConfigureAwait(false);
    }
}
