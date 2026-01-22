using Core.Models.Match;

namespace Core.Interfaces.Infrastructure
{
    public interface IRedisLockService
    {
        Task<RedisLockHandle?> AcquireAsync(string key);
        Task ReleaseAsync(RedisLockHandle handle);
    }
}
