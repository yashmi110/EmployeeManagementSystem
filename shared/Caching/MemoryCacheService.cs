using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Shared.Caching;

public class MemoryCacheService : ICacheService
{
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<MemoryCacheService> _logger;

    public MemoryCacheService(IMemoryCache memoryCache, ILogger<MemoryCacheService> logger)
    {
        _memoryCache = memoryCache;
        _logger = logger;
    }

    public Task<T?> GetAsync<T>(string key)
    {
        try
        {
            var value = _memoryCache.Get<T>(key);
            return Task.FromResult(value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving value from memory cache for key: {Key}", key);
            return Task.FromResult<T?>(default);
        }
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        try
        {
            var options = new MemoryCacheEntryOptions();
            if (expiration.HasValue)
                options.AbsoluteExpirationRelativeToNow = expiration;

            _memoryCache.Set(key, value, options);
            _logger.LogDebug("Value cached in memory with key: {Key}, expiration: {Expiration}", key, expiration);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error setting value in memory cache for key: {Key}", key);
        }

        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key)
    {
        try
        {
            _memoryCache.Remove(key);
            _logger.LogDebug("Cache entry removed from memory with key: {Key}", key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing value from memory cache for key: {Key}", key);
        }

        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(string key)
    {
        try
        {
            var exists = _memoryCache.TryGetValue(key, out _);
            return Task.FromResult(exists);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking existence in memory cache for key: {Key}", key);
            return Task.FromResult(false);
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

    public Task RemoveByPatternAsync(string pattern)
    {
        // Note: Memory cache doesn't support pattern-based deletion
        // This would require additional logic to track keys
        _logger.LogWarning("Pattern-based cache removal not implemented for memory cache. Pattern: {Pattern}", pattern);
        return Task.CompletedTask;
    }
} 