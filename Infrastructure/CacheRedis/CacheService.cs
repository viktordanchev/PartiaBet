using Core.Enums;
using Core.Interfaces.Infrastructure;
using Core.Models.Match;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Text.Json;

namespace Infrastructure.CacheRedis
{
    public class CacheService : ICacheService
    {
        private readonly StackExchange.Redis.IDatabase _redis;

        public CacheService(IConnectionMultiplexer mux)
        {
            _redis = mux.GetDatabase(); 
        }

        public async Task SetMatchAsync(Guid matchId, MatchModel match)
        {
            var json = JsonSerializer.Serialize(match);

            await _redis.StringSetAsync(
                matchId.ToString(),
                json
            );

            if (match.Status != MatchStatus.Finished)
            {
                await _redis.SetAddAsync(
                    $"{match.GameType}",
                    matchId.ToString()
                );
            }
        }

        public async Task<MatchModel> GetMatchAsync(Guid matchId)
        {
            var gameBoardJSON = await _redis.StringGetAsync(matchId.ToString());
            var gameBoard = JsonSerializer.Deserialize<MatchModel>(gameBoardJSON);

            return gameBoard;
        }

        public async Task<IEnumerable<MatchModel>> GetAllMatchesAsync(GameType gameType)
        {
            var ids = await _redis.SetMembersAsync($"{gameType}");

            var keys = ids.Select(id => (RedisKey)$"{id}").ToArray();
            var values = await _redis.StringGetAsync(keys);

            var matches = values
                .Where(v => !v.IsNull)
                .Select(v => JsonSerializer.Deserialize<MatchModel>(v))
                .Where(m => m != null)
                .Select(m => m!)
                .ToList();

            return matches;
        }
    }
}
