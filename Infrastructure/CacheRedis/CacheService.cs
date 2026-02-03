using Core.Enums;
using Core.Interfaces.Infrastructure;
using Core.Models.Match;
using StackExchange.Redis;
using System.Text.Json;

namespace Infrastructure.CacheRedis
{
    public class CacheService : ICacheService
    {
        private readonly IDatabase _redis;

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

            await _redis.SetAddAsync(
                $"{match.GameType}",
                matchId.ToString()
            );
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

        public async Task SetPlayerMatchAsync(Guid playerId, Guid matchId)
        {
            await _redis.StringSetAsync($"{playerId}", matchId.ToString());
        }

        public async Task<Guid> GetPlayerMatchIdAsync(Guid playerId)
        {
            var matchIdString = await _redis.StringGetAsync($"{playerId}");
            if (matchIdString.IsNullOrEmpty) return Guid.Empty;

            return Guid.Parse(matchIdString);
        }

        public async Task RemoveMatchAsync(Guid matchId, GameType gameType)
        {
            // 1. Махаш самия мач
            await _redis.KeyDeleteAsync(matchId.ToString());

            // 2. Махаш го от сета по тип игра
            await _redis.SetRemoveAsync(
                gameType.ToString(),
                matchId.ToString()
            );
        }
    }
}
