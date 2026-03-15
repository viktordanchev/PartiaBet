using Core.Interfaces.Infrastructure;
using StackExchange.Redis;

namespace Infrastructure.CacheRedis
{
    public class OnlineUsersCache : IOnlineUsersCache
    {
        private readonly IDatabase _redis;

        public OnlineUsersCache(IConnectionMultiplexer mux)
        {
            _redis = mux.GetDatabase();
        }

        public async Task SetUserOnlineAsync(Guid userId)
        {
            await _redis.StringSetAsync($"online:user:{userId}", "1", TimeSpan.FromSeconds(60));
            await _redis.SetAddAsync("online:users", userId.ToString());
        }

        public async Task SetUserOfflineAsync(Guid userId)
        {
            await _redis.KeyDeleteAsync($"online:user:{userId}");
            await _redis.SetRemoveAsync("online:users", userId.ToString());
        }

        public async Task<IEnumerable<Guid>> GetOnlineUsersAsync(IEnumerable<Guid> userIds)
        {
            var idsArray = userIds.ToArray();
            var keys = idsArray.Select(id => (RedisKey)$"online:user:{id}").ToArray();

            var values = await _redis.StringGetAsync(keys);

            var onlineUsers = new List<Guid>();
            for (int i = 0; i < idsArray.Length; i++)
            {
                if (!values[i].IsNull)
                {
                    onlineUsers.Add(idsArray[i]);
                }
            }

            return onlineUsers;
        }
    }
}
