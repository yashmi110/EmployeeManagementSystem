using Microsoft.Extensions.Logging;

namespace Shared.Caching;

public abstract class BaseCachedRepository
{
    protected readonly ICacheService _cacheService;
    protected readonly ILogger _logger;
    protected readonly TimeSpan _defaultExpiration = TimeSpan.FromMinutes(30);

    protected BaseCachedRepository(ICacheService cacheService, ILogger logger)
    {
        _cacheService = cacheService;
        _logger = logger;
    }

    protected async Task<T?> GetFromCacheAsync<T>(string key, Func<Task<T?>> factory)
    {
        return await _cacheService.GetOrSetAsync(key, factory, _defaultExpiration);
    }

    protected async Task SetCacheAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        await _cacheService.SetAsync(key, value, expiration ?? _defaultExpiration);
    }

    protected async Task RemoveCacheAsync(string key)
    {
        await _cacheService.RemoveAsync(key);
    }

    protected async Task InvalidateMultipleCachesAsync(params string[] keys)
    {
        var tasks = keys.Select(key => _cacheService.RemoveAsync(key));
        await Task.WhenAll(tasks);
        _logger.LogDebug("Multiple cache entries invalidated: {Keys}", string.Join(", ", keys));
    }
} 