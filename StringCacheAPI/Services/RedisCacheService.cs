using StackExchange.Redis;

namespace StringCacheAPI.Services
{
    public interface IRedisCacheService
    {
        Task<string?> Get(string id);
        Task Set(string id, string data, int ttl);
        Task<bool> Update(string id, string data);
        Task<bool> Delete(string id);
    }

    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDatabase _database;

        public RedisCacheService(IDatabase database)
        {
            _database = database;
        }

        public async Task<string?> Get(string id)
        {
            var result = await _database.StringGetAsync(key: id);
            return !result.HasValue ? null : result.ToString();
        }

        public async Task Set(string id, string data, int ttl)
        {
            await _database.StringSetAsync(key: id, value: data, expiry: TimeSpan.FromSeconds(ttl));
        }

        public async Task<bool> Update(string id, string data)
        {
            if (!await _database.KeyExistsAsync(id))
                return false;

            await _database.StringSetAsync(key: id, value: data);
            return true;
        }

        public async Task<bool> Delete(string id)
        {
            var result = await _database.StringGetDeleteAsync(key: id);

            return result.HasValue;
        }
    }
}
