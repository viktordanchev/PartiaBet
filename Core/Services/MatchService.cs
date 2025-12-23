using Core.Enums;
using Core.Interfaces.Games;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models.Match;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Core.Services
{
    public class MatchService : IMatchService
    {
        private IMatchRepository _matchRepository;
        private IDistributedCache _redis;
        private IGameFactory _gameFactory;

        public MatchService(IMatchRepository matchRepository, IDistributedCache redis, IGameFactory gameFactory)
        {
            _matchRepository = matchRepository;
            _redis = redis;
            _gameFactory = gameFactory;
        }

        public async Task<IEnumerable<MatchModel>> GetActiveMatchesAsync(int gameId)
        {
            return await _matchRepository.GetActiveMatchesAsync(gameId);
        }

        public async Task<MatchModel> CreateMatchAsync(AddMatchModel data)
        {
            var match = await _matchRepository.AddMatchAsync(data);
            var gameService = _gameFactory.GetGameService(GameType.Chess);

            var model = gameService.CreateGameBoard();
            var gameBoardJSON = JsonSerializer.Serialize(model);
            await _redis.SetStringAsync(match.Id.ToString(), gameBoardJSON);

            return match;
        }

        public async Task AddPersonToMatch(Guid matchId, Guid playerId)
        {
            await _matchRepository.TryAddPlayerToMatchAsync(playerId, matchId);
        }

        public async Task<MatchDetailsModel> GetMatch(Guid matchId)
        {
            var match = await _matchRepository.GetMatchDetailsAsync(matchId);
            var gameBoardJSON = await _redis.GetStringAsync(matchId.ToString());

            var gameBoard = JsonSerializer.Deserialize<GameBoardModel>(gameBoardJSON);
            match.Board = gameBoard;
            return match;
        }

        public async Task TryMakeMove(Guid matchId, BaseMoveModel moveData)
        {

        }

        public async Task<GameType> GetMatchGameTypeAsync(Guid matchId)
        {
            var gameId = await _matchRepository.GetGameIdAsync(matchId);

            return (GameType)gameId;
        }
    }
}
