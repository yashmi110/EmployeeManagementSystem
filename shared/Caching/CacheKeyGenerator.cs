namespace Shared.Caching;

public static class CacheKeyGenerator
{
    public static string GenerateKey(string service, string entity, string operation, params object[] parameters)
    {
        var keyParts = new List<string> { service, entity, operation };
        keyParts.AddRange(parameters.Select(p => p?.ToString() ?? "null"));
        
        return string.Join(":", keyParts).ToLowerInvariant();
    }

    public static string GenerateListKey(string service, string entity, string filter = "")
    {
        var key = $"{service}:{entity}:list";
        if (!string.IsNullOrEmpty(filter))
            key += $":{filter}";
        
        return key.ToLowerInvariant();
    }

    public static string GenerateByIdKey(string service, string entity, int id)
    {
        return $"{service}:{entity}:{id}".ToLowerInvariant();
    }

    public static string GenerateByFilterKey(string service, string entity, string filterType, string filterValue)
    {
        return $"{service}:{entity}:filter:{filterType}:{filterValue}".ToLowerInvariant();
    }

    public static string GenerateStatsKey(string service, string entity, string statType)
    {
        return $"{service}:{entity}:stats:{statType}".ToLowerInvariant();
    }
} 