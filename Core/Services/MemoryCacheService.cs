using Core.Interfaces.Service;
using Microsoft.Extensions.Caching.Memory;

namespace Core.Services
{
    public class MemoryCacheService : IMemoryCacheService
    {
        private IMemoryCache _memoryCache;

        public MemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public void Add(string key, string value, TimeSpan time)
        {
            _memoryCache.Set(key, value, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = time
            });
        }

        public bool isExist(string key)
        {
            return _memoryCache.Get(key) != null;
        }

        public string GetValue(string key)
        {
            return _memoryCache.Get(key) != null ? _memoryCache.Get(key).ToString() : string.Empty;
        }
    }
}