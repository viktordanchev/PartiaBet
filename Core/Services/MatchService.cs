using Core.Enums;
using Core.Interfaces.Games;
using Core.Interfaces.Infrastructure;
using Core.Interfaces.Services;
using Core.Models.Games;
using Core.Models.Match;
using System.Text.Json;

namespace Core.Services
{
    public class MatchService : IMatchService
    {
        private IMatchRepository _matchRepository;
        private ICacheService _cacheService;
        private IGameFactory _gameFactory;
        private IMatchTimer _matchTimerManager;
        private IGameProvider _gameProvider;

        public MatchService(
            IMatchRepository matchRepository,
            ICacheService cacheService,
            IGameFactory gameFactory,
            IMatchTimer matchTimerManager,
            IGameProvider gameProvider)
        {
            _matchRepository = matchRepository;
            _cacheService = cacheService;
            _gameFactory = gameFactory;
            _matchTimerManager = matchTimerManager;
            _gameProvider = gameProvider;
        }

        public async Task<IEnumerable<MatchModel>> GetActiveMatchesAsync(GameType gameType)
        {
            var matches = await _matchRepository.GetActiveMatchesAsync(gameType);
            var maxPlayersInMatch = _gameProvider.GetMaxPlayersCount(gameType);

            foreach (var match in matches)
            {
                match.MaxPlayersCount = maxPlayersInMatch;
            }

            return matches;
        }

        public async Task<MatchModel> CreateMatchAsync(AddMatchModel data)
        {
            var match = await _matchRepository.AddMatchAsync(data);
            var gameService = _gameFactory.GetGameService(data.GameType);

            var gameBoard = gameService.CreateGameBoard();
            await _cacheService.AddItem(match.Id, gameBoard);

            match.MaxPlayersCount = _gameProvider.GetMaxPlayersCount(data.GameType);

            return match;
        }

        public async Task<PlayerModel> AddPlayerAsync(Guid matchId, Guid playerId)
        {
            var match = await _matchRepository.GetMatchInternalAsync(matchId);
            var gameBoard = await _cacheService.GetItem(matchId);
            var gameService = _gameFactory.GetGameService(match.GameType);
            var addedPlayer = await _matchRepository.AddPlayerAsync(playerId, matchId);

            gameService.UpdatePlayersInBoard(gameBoard, playerId);
            await _cacheService.AddItem(matchId, gameBoard);

            return addedPlayer;
        }

        public async Task RemovePlayerAsync(Guid matchId, Guid playerId)
        {
            var match = await _matchRepository.GetMatchInternalAsync(matchId);

            if (match.MatchStatus == MatchStatus.Ongoing)
            {
                _matchTimerManager.StartTimer(matchId, playerId, TimeSpan.FromMinutes(5));
            }
            else
            {
                var gameBoard = await _cacheService.GetItem(matchId);
                var gameService = _gameFactory.GetGameService(match.GameType);

                gameService.UpdatePlayersInBoard(gameBoard, playerId);
                await _matchRepository.RemovePlayerAsync(playerId, matchId);
            }
        }

        public async Task<MatchModel> GetMatchAsync(Guid matchId)
        {
            var match = await _matchRepository.GetMatchAsync(matchId);
            var gameBoard = await _cacheService.GetItem(matchId);

            match.Board = gameBoard;
            return match;
        }

        public async Task<(bool, BaseMoveModel)> TryMakeMoveAsync(Guid matchId, Guid playerId, string moveJson)
        {
            var match = await _matchRepository.GetMatchInternalAsync(matchId);
            var gameBoard = await _cacheService.GetItem(matchId);

            var moveData = _gameFactory.GetMakeMoveDto(match.GameType, moveJson);
            var gameService = _gameFactory.GetGameService(match.GameType);
            var isValidMove = gameService.IsValidMove(gameBoard, moveData);

            if (isValidMove)
            {
                gameService.UpdateBoard(gameBoard, moveData);
                await _cacheService.AddItem(matchId, gameBoard);
            }

            return (isValidMove, moveData);
        }
    }
}
