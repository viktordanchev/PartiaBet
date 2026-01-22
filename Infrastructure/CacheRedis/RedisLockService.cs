using Core.Interfaces.Infrastructure;
using Core.Models.Match;
using StackExchange.Redis;

namespace Infrastructure.CacheRedis
{
    public sealed class RedisLockService : IRedisLockService
    {
        private readonly IDatabase _redis;

        private const string ReleaseScript = @"
        if redis.call('GET', KEYS[1]) == ARGV[1] then
            return redis.call('DEL', KEYS[1])
        else
            return 0
        end";

        public RedisLockService(IConnectionMultiplexer mux)
        {
            _redis = mux.GetDatabase();
        }

        public async Task<RedisLockHandle?> AcquireAsync(string key)
        {
            var value = Guid.NewGuid().ToString();
            var start = DateTime.UtcNow;
            var waitTimeout = TimeSpan.FromSeconds(3);
            var retryDelay = TimeSpan.FromMilliseconds(100);
            var expiry = TimeSpan.FromSeconds(5);

            while (DateTime.UtcNow - start < waitTimeout)
            {
                var acquired = await _redis.StringSetAsync(
                    key,
                    value,
                    expiry,
                    When.NotExists);

                if (acquired)
                {
                    return new RedisLockHandle(key, value);
                }

                await Task.Delay(retryDelay);
            }

            return null;
        }

        public async Task ReleaseAsync(RedisLockHandle handle)
        {
            if (handle == null)
                return;

            await _redis.ScriptEvaluateAsync(
                ReleaseScript,
                new RedisKey[] { handle.Key },
                new RedisValue[] { handle.Value });
        }
    }
}
