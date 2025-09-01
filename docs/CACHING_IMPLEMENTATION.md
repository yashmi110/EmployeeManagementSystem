# Redis Caching Implementation Guide

## Overview

This document describes the Redis caching implementation for the Department API microservices architecture. The implementation provides multiple caching strategies with Redis as the primary distributed cache and in-memory cache as a fallback.

## Architecture

### Caching Layers

1. **Redis Cache (Primary)**: Distributed cache for cross-service data sharing
2. **Memory Cache (Fallback)**: Local in-memory cache when Redis is unavailable
3. **Repository Caching**: Caching decorators wrapping repository implementations

### Cache Service Interface

```csharp
public interface ICacheService
{
    Task<T?> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);
    Task RemoveAsync(string key);
    Task<bool> ExistsAsync(string key);
    Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null);
    Task RemoveByPatternAsync(string pattern);
}
```

## Implementation Details

### 1. Cache Key Generation

Cache keys follow a consistent naming convention:
- **List Operations**: `{service}:{entity}:list`
- **ById Operations**: `{service}:{entity}:{id}`
- **Filtered Operations**: `{service}:{entity}:filter:{filterType}:{filterValue}`
- **Statistics**: `{service}:{entity}:stats:{statType}`

Examples:
- `departmentservice:department:list`
- `employeeservice:employee:123`
- `employeeservice:employee:filter:department:engineering`
- `employeeservice:employee:stats:average-age`

### 2. Caching Strategies

#### Read Operations
- **Cache-First**: Check cache before hitting database
- **Cache-Aside**: Load data into cache after database query
- **TTL-based Expiration**: Configurable expiration times

#### Write Operations
- **Write-Through**: Update cache immediately after database write
- **Cache Invalidation**: Remove related cache entries on data changes
- **Bulk Invalidation**: Clear multiple related caches on major changes

### 3. Cache Expiration

- **Default Expiration**: 30 minutes for most data
- **Statistics Expiration**: 15 minutes for calculated values
- **Configurable**: Per-operation expiration override support

## Configuration

### Redis Connection

```json
{
  "ConnectionStrings": {
    "RedisConnection": "localhost:6379"
  }
}
```

### Service Registration

```csharp
// Configure Caching
builder.Services.AddMemoryCache();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("RedisConnection") ?? "localhost:6379";
    options.InstanceName = "DepartmentService_";
});

// Register cache services
builder.Services.AddScoped<ICacheService, RedisCacheService>();
builder.Services.AddScoped<ICacheService, MemoryCacheService>();

// Decorate repositories with caching
builder.Services.Decorate<IDepartmentRepository, CachedDepartmentRepository>();
```

## Performance Optimizations

### 1. Asynchronous Operations
- All cache operations are async
- Non-blocking cache access
- Parallel cache invalidation

### 2. Lazy Loading
- Cache miss triggers database query
- Automatic cache population
- Background cache updates

### 3. Indexing Support
- Database indexes for frequently queried fields
- Composite indexes for complex queries
- Query optimization for cached data

### 4. Response Headers
- `X-Cache-Status`: HIT/MISS indicator
- `X-Cache-Provider`: Cache provider used
- `X-Response-Time`: Request processing time

## Monitoring and Debugging

### Cache Hit/Miss Logging
```csharp
_logger.LogDebug("Cache hit for key: {Key}", key);
_logger.LogDebug("Cache miss for key: {Key}, executing factory", key);
```

### Performance Metrics
- Response time tracking
- Cache hit ratio monitoring
- Database query reduction

### Health Checks
- Redis connectivity monitoring
- Cache service availability
- Fallback mechanism status

## Usage Examples

### Basic Caching
```csharp
public async Task<DepartmentDto?> GetByIdAsync(int id)
{
    var cacheKey = CacheKeyGenerator.GenerateByIdKey("departmentservice", "department", id);
    
    return await _cacheService.GetOrSetAsync(cacheKey, async () =>
    {
        return await _repository.GetByIdAsync(id);
    }, TimeSpan.FromMinutes(30));
}
```

### Cache Invalidation
```csharp
public async Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto)
{
    var result = await _repository.CreateAsync(dto);
    
    // Invalidate list cache and add new item
    await InvalidateListCache();
    var cacheKey = CacheKeyGenerator.GenerateByIdKey("departmentservice", "department", result.Id);
    await _cacheService.SetAsync(cacheKey, result, _defaultExpiration);
    
    return result;
}
```

## Deployment

### Docker Compose
```bash
# Start Redis
docker-compose -f docker-compose.redis.yml up -d

# Check Redis health
docker exec department-api-redis redis-cli ping
```

### Environment Variables
```bash
# Redis Configuration
REDIS_CONNECTION=localhost:6379
REDIS_INSTANCE_NAME=DepartmentService_
CACHE_DEFAULT_EXPIRATION=1800  # 30 minutes in seconds
CACHE_STATS_EXPIRATION=900     # 15 minutes in seconds
```

## Best Practices

### 1. Cache Key Design
- Use consistent naming conventions
- Include service and entity identifiers
- Avoid overly long keys

### 2. Expiration Strategy
- Set appropriate TTL values
- Use shorter expiration for frequently changing data
- Implement cache warming for critical data

### 3. Error Handling
- Graceful fallback to database on cache failures
- Log cache errors for monitoring
- Implement circuit breaker pattern

### 4. Memory Management
- Monitor Redis memory usage
- Implement eviction policies
- Use compression for large objects

## Troubleshooting

### Common Issues

1. **Redis Connection Failures**
   - Check Redis service status
   - Verify connection string
   - Check firewall settings

2. **Cache Misses**
   - Verify cache key generation
   - Check expiration settings
   - Monitor cache population

3. **Performance Issues**
   - Analyze cache hit ratios
   - Check Redis memory usage
   - Monitor response times

### Debug Commands
```bash
# Redis CLI
docker exec -it department-api-redis redis-cli

# Monitor Redis operations
MONITOR

# Check memory usage
INFO memory

# List all keys
KEYS *
```

## Future Enhancements

1. **Cache Warming**: Pre-populate frequently accessed data
2. **Compression**: Implement data compression for large objects
3. **Clustering**: Redis cluster support for high availability
4. **Metrics**: Integration with monitoring systems
5. **Backup**: Automated cache backup and recovery 