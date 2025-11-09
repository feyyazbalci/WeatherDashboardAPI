using System.Text.Json;
using StackExchange.Redis;

namespace WeatherDashboardAPI.Services
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _database;
        private readonly ILogger<RedisCacheService> _logger;

        public RedisCacheService(
            IConnectionMultiplexer redis,
            ILogger<RedisCacheService> logger
        )
        {
            _redis = redis;
            _database = _redis.GetDatabase();
            _logger = logger;
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            try
            {
                var value = await _database.StringGetAsync(key);
                if (value.IsNullOrEmpty)
                {
                    _logger.LogDebug("Cache MISS: {Key}", key);
                    return default;
                }
                _logger.LogDebug("Cache HIT: {KEY}", key);
                return JsonSerializer.Deserialize<T>(value!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Redis GET error: {Key}", key);
                return default;
            }
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            try
            {
                var serializedValue = JsonSerializer.Serialize(value);

                if (expiration.HasValue)
                {
                    await _database.StringSetAsync(key, serializedValue, expiration.Value);
                    _logger.LogDebug("Cache SET: {Key} (Expires in {Minutes} minutes)",
                        key, expiration.Value.TotalMinutes);
                }
                else
                {
                    await _database.StringSetAsync(key, serializedValue);
                    _logger.LogDebug("Cache SET: {Key} (No expiration)", key);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Redis SET hatası: {Key}", key);
            }
        }

        public async Task RemoveAsync(string key)
        {
            try
            {
                await _database.KeyDeleteAsync(key);
                _logger.LogDebug("Cache REMOVE: {Key}", key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Redis REMOVE error: {Key}", key);
            }
        }
        public async Task<bool> ExistsAsync(string key)
        {
            try
            {
                return await _database.KeyExistsAsync(key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Redis EXISTS error: {Key}", key);
                return false;
            }
        }
    }
}