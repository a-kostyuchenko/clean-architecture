using System.Collections.Concurrent;
using Application.Abstractions.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Infrastructure.Caching;

public class CacheService(IDistributedCache distributedCache) : ICacheService
{
    private static readonly ConcurrentDictionary<string, bool> CacheKeys = new();
    private static readonly TimeSpan DefaultExpiration = TimeSpan.FromMinutes(5);

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        string? cachedValue = await distributedCache.GetStringAsync(
            key,
            cancellationToken);

        if (cachedValue is null)
            return default;

        var value = JsonConvert.DeserializeObject<T>(cachedValue);

        return value;
    }

    public async Task<T> GetAsync<T>(
        string key,
        Func<CancellationToken, Task<T>> factory,
        TimeSpan? expiration = null,
        CancellationToken cancellationToken = default)
    {
        var cachedValue = await GetAsync<T>(key, cancellationToken);

        if (cachedValue is not null)
            return cachedValue;

        cachedValue = await factory(cancellationToken);

        await SetAsync(key, cachedValue, expiration, cancellationToken);

        return cachedValue;
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration, CancellationToken cancellationToken = default)
    {
        string cacheValue = JsonConvert.SerializeObject(value);

        await distributedCache.SetStringAsync(
            key,
            cacheValue,
            new DistributedCacheEntryOptions{ AbsoluteExpirationRelativeToNow = expiration ?? DefaultExpiration },
            cancellationToken);

        CacheKeys.TryAdd(key, false);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await distributedCache.RemoveAsync(key, cancellationToken);

        CacheKeys.TryRemove(key, out bool _);
    }

    public async Task RemoveByPrefixAsync(string prefixKey, CancellationToken cancellationToken = default)
    {
        IEnumerable<Task> tasks = CacheKeys
            .Keys
            .Where(k => k.StartsWith(prefixKey))
            .Select(k => RemoveAsync(k, cancellationToken));

        await Task.WhenAll(tasks);
    }
}