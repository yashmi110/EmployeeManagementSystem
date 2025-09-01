using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Shared.Caching;

public class RedisCacheService : ICacheService
{
    private readonly IDistributedCache _distributedCache;
    private readonly ILogger<RedisCacheService> _logger;

    public RedisCacheService(IDistributedCache distributedCache, ILogger<RedisCacheService> logger)
    {
        _distributedCache = distributedCache;
        _logger = logger;
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        try
        {
            var cachedValue = await _distributedCache.GetStringAsync(key);
            if (string.IsNullOrEmpty(cachedValue))
                return default;

            return JsonSerializer.Deserialize<T>(cachedValue);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving value from Redis cache for key: {Key}", key);
            return default;
        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        try
        {
            var options = new DistributedCacheEntryOptions();
            if (expiration.HasValue)
                options.SetAbsoluteExpiration(expiration.Value);

            var serializedValue = JsonSerializer.Serialize(value);
            await _distributedCache.SetStringAsync(key, serializedValue, options);
            
            _logger.LogDebug("Value cached in Redis with key: {Key}, expiration: {Expiration}", key, expiration);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error setting value in Redis cache for key: {Key}", key);
        }
    }

    public async Task RemoveAsync(string key)
    {
        try
        {
            await _distributedCache.RemoveAsync(key);
            _logger.LogDebug("Cache entry removed from Redis with key: {Key}", key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing value from Redis cache for key: {Key}", key);
        }
    }

    public async Task<bool> ExistsAsync(string key)
    {
        try
        {
            var cachedValue = await _distributedCache.GetStringAsync(key);
            return !string.IsNullOrEmpty(cachedValue);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking existence in Redis cache for key: {Key}", key);
            return false;
        }
    }

    public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null)
    {
        var cachedValue = await GetAsync<T>(key);
        if (cachedValue != null)
        {
            _logger.LogDebug("Cache hit for key: {Key}", key);
            return cachedValue;
        }

        _logger.LogDebug("Cache miss for key: {Key}, executing factory", key);
        var value = await factory();
        await SetAsync(key, value, expiration);
        return value;
    }

    public async Task RemoveByPatternAsync(string pattern)
    {
        // Note: Redis doesn't support pattern-based deletion natively
        // This would require additional Redis commands or a different approach
        // For now, we'll log a warning
        _logger.LogWarning("Pattern-based cache removal not implemented for Redis. Pattern: {Pattern}", pattern);
        await Task.CompletedTask;
    }
} 