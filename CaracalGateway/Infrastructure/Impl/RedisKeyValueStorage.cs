using StackExchange.Redis;
using System.Text.Json;

namespace CaracalGateway.Infrastructure
{
    public class RedisKeyValueStorage : IKeyValueStorage, IDisposable
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private IDatabase _database => _connectionMultiplexer.GetDatabase();

        public RedisKeyValueStorage(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }

        public async Task AddEntranceAsync(string key, object value, int? lifetime = null)
        {
            var stringValue = JsonSerializer.Serialize(value);
            await _database.StringSetAsync(key, stringValue);
        }

        public async Task<T> GetValueAsync<T>(string key)
        {
            var redisValue = await _database.StringGetAsync(key);
            if (!redisValue.HasValue)
            {
                return default(T);
            }
            var value = JsonSerializer.Deserialize<T>(redisValue.ToString());
            return value;
        }

        public async Task RemoveEntranceAsync(string key) => await _database.KeyDeleteAsync(key);
        
        public void Dispose()
        {
            _connectionMultiplexer.Dispose();
        }
    }
}
