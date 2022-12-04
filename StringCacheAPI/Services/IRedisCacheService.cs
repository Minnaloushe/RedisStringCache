namespace StringCacheAPI.Services;

public interface IRedisCacheService
{
    Task<string?> Get(string id);
    Task Set(string id, string data, int ttl);
    Task<bool> Update(string id, string data);
    Task<bool> Delete(string id);
}