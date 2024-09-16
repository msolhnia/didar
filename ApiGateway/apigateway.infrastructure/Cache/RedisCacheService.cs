using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apigateway.infrastructure.Cache
{
    public class RedisCacheService
    {
        private readonly IDatabase _redisDb;

        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _redisDb = redis.GetDatabase();
        }

        public async Task SetModuleCacheAsync(string key, string data)
        {
            await _redisDb.StringSetAsync(key, data);
        }

        public async Task<string> GetModuleCacheAsync(string key)
        {
            return await _redisDb.StringGetAsync(key);
        }

        public async Task RemoveModuleCacheAsync(string key)
        {
            await _redisDb.KeyDeleteAsync(key);
        }
    }

}
