using Core.Enums;
using Core.Interfaces.Games;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Models.Match;

namespace Core.Services
{
    public class MatchService : IMatchService
    {
        private IMatchRepository _matchRepository;
        private ICacheService _cacheService;
        private IGameFactory _gameFactory;

        public MatchService(IMatchRepository matchRepository, ICacheService cacheService, IGameFactory gameFactory)
        {
            _matchRepository = matchRepository;
            _cacheService = cacheService;
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
            model.MaxPlayersCount = match.MaxPlayersCount;
            await _cacheService.AddItem(match.Id, model);

            return match;
        }

        public async Task<PlayerModel> AddPersonToMatch(Guid matchId, Guid playerId)
        {
            var gameBoard = await _cacheService.GetItem(matchId);
            var playersCount = await _matchRepository.GetPlayersCountAsync(matchId);
            var gameType = await GetMatchGameTypeAsync(matchId);
            var gameService = _gameFactory.GetGameService(gameType);
            var addedPlayer = await _matchRepository.TryAddPlayerToMatchAsync(playerId, matchId);

            gameService.AddPlayerToBoard(gameBoard, playerId, playersCount);

            return addedPlayer;
        }

        public async Task<MatchDetailsModel> GetMatch(Guid matchId)
        {
            var match = await _matchRepository.GetMatchDetailsAsync(matchId);
            var gameBoard = await _cacheService.GetItem(matchId);

            match.Board = gameBoard;
            return match;
        }

        public async Task TryMakeMove(Guid matchId, GameType game, string playerId, BaseMoveModel moveData)
        {
            var gameService = _gameFactory.GetGameService(game);
            var gameBoard = await _cacheService.GetItem(matchId);

            if (gameService.IsValidMove(gameBoard, moveData))
            {
                gameService.UpdateBoard(gameBoard, moveData);

                await _cacheService.AddItem(matchId, gameBoard);
            }
        }

        public async Task<GameType> GetMatchGameTypeAsync(Guid matchId)
        {
            var gameId = await _matchRepository.GetGameIdAsync(matchId);

            return (GameType)gameId;
        }
    }
}
