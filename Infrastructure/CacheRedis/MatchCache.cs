using Core.Enums;
using Core.Interfaces.Infrastructure;
using Core.Models.Match;
using StackExchange.Redis;
using System.Text.Json;

namespace Infrastructure.CacheRedis
{
    public class MatchCache : IMatchCache
    {
        private readonly IDatabase _redis;

        public MatchCache(IConnectionMultiplexer mux)
        {
            _redis = mux.GetDatabase();
        }

        //Set methods

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

        public async Task SetPlayerMatchAsync(Guid playerId, Guid matchId)
        {
            await _redis.StringSetAsync($"{playerId}", matchId.ToString());
        }

        //Get methods

        public async Task<MatchModel?> GetMatchAsync(Guid matchId)
        {
            var matchJson = await _redis.StringGetAsync(matchId.ToString());

            if (string.IsNullOrEmpty(matchJson))
                return null;

            return JsonSerializer.Deserialize<MatchModel>(matchJson);
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

        public async Task<Guid> GetPlayerMatchIdAsync(Guid playerId)
        {
            var matchIdString = await _redis.StringGetAsync($"{playerId}");
            if (matchIdString.IsNullOrEmpty) return Guid.Empty;

            return Guid.Parse(matchIdString);
        }

        //Delete methods

        public async Task RemoveMatchAsync(MatchModel match)
        {
            await _redis.KeyDeleteAsync(match.Id.ToString());

            await _redis.SetRemoveAsync(
                match.GameType.ToString(),
                match.Id.ToString()
            );

            foreach (var player in match.Players)
            {
                await _redis.KeyDeleteAsync(player.Id.ToString());
            }
        }
    }
}
