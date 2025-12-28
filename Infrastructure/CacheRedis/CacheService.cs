using Core.Interfaces.Repositories;
using Core.Models.Match;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Infrastructure.CacheRedis
{
    public class CacheService : ICacheService
    {
        private IDistributedCache _redis;

        public CacheService(IDistributedCache redis)
        {
            _redis = redis; 
        }

        public async Task AddItem(Guid matchId, GameBoardModel gameBoard)
        { 
            var gameBoardJSON = JsonSerializer.Serialize(gameBoard);

            await _redis.SetStringAsync(matchId.ToString(), gameBoardJSON);
        }

        public async Task<GameBoardModel> GetItem(Guid matchId)
        {
            var gameBoardJSON = await _redis.GetStringAsync(matchId.ToString());
            var gameBoard = JsonSerializer.Deserialize<GameBoardModel>(gameBoardJSON);

            return gameBoard;
        }
    }
}
