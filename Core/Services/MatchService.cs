using Core.Enums;
using Core.Interfaces.Games;
using Core.Interfaces.Infrastructure;
using Core.Interfaces.Services;
using Core.Models.Games;
using Core.Models.Match;

namespace Core.Services
{
    public class MatchService : IMatchService
    {
        private IMatchRepository _matchRepository;
        private ICacheService _cacheService;
        private IGameFactory _gameFactory;
        private IGameProvider _gameProvider;

        public MatchService(
            IMatchRepository matchRepository,
            ICacheService cacheService,
            IGameFactory gameFactory,
            IGameProvider gameProvider)
        {
            _matchRepository = matchRepository;
            _cacheService = cacheService;
            _gameFactory = gameFactory;
            _gameProvider = gameProvider;
        }

        public async Task<MatchModel> GetMatchInternalAsync(Guid matchId)
        {
            var match = await _matchRepository.GetMatchInternalAsync(matchId);
            match.MaxPlayersCount = _gameProvider.GetMaxPlayersCount(match.GameType);

            return match;
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

        public async Task<PlayerModel> AddPlayerAsync(MatchModel match, Guid playerId)
        {
            if (match.MatchStatus == MatchStatus.Ongoing || match.Players.Any(p => p.Id == playerId))
            {
                throw new InvalidOperationException("Cannot join an ongoing match.");
            }

            var gameBoard = await _cacheService.GetItem(match.Id);
            var gameService = _gameFactory.GetGameService(match.GameType);
            var addedPlayer = await _matchRepository.AddPlayerAsync(playerId, match.Id);

            gameService.UpdatePlayersInBoard(gameBoard, playerId);
            await _cacheService.AddItem(match.Id, gameBoard);

            if (match.MaxPlayersCount == match.Players.Count + 1)
            {
                await _matchRepository.UpdateMatchStatusAsync(match.Id, MatchStatus.Ongoing);
            }

            return addedPlayer;
        }

        public async Task UpdatePlayerStatusAsync(Guid matchId, Guid playerId, PlayerStatus status)
        {
            await _matchRepository.UpdatePlayerStatusAsync(matchId, playerId, status);
        }

        public async Task RemovePlayerAsync(MatchModel match, Guid playerId)
        {
            var gameBoard = await _cacheService.GetItem(match.Id);
            var gameService = _gameFactory.GetGameService(match.GameType);

            gameService.UpdatePlayersInBoard(gameBoard, playerId);
            await _matchRepository.RemovePlayerAsync(playerId, match.Id);
        }

        public async Task<MatchModel> GetMatchAsync(Guid matchId)
        {
            var match = await _matchRepository.GetMatchAsync(matchId);
            var gameBoard = await _cacheService.GetItem(matchId);

            match.Board = gameBoard;
            return match;
        }

        public async Task<MoveResultModel> TryMakeMoveAsync(MatchModel match, Guid playerId, string moveJson)
        {
            var gameBoard = await _cacheService.GetItem(match.Id);

            var moveData = _gameFactory.GetMakeMoveDto(match.GameType, moveJson);
            var gameService = _gameFactory.GetGameService(match.GameType);
            var isValidMove = gameService.IsValidMove(gameBoard, moveData);

            if (!isValidMove)
            {
                return MoveResultModel.Invalid();
            }
            else if (gameService.IsWinningMove(gameBoard))
            {
                return MoveResultModel.Win(playerId);
            }

            gameService.UpdateBoard(gameBoard, moveData);
            await _cacheService.AddItem(match.Id, gameBoard);

            return MoveResultModel.Success(moveData);
        }

        public async Task<Guid> SwtichTurnAsync(MatchModel match, Guid currentPlayerId)
        {
            var gameService = _gameFactory.GetGameService(GameType.Chess);
            var nextPlayerId = gameService.SwitchTurn(currentPlayerId, match.Players);

            await _matchRepository.UpdatePlayerIdAsync(match.Id, nextPlayerId);

            return nextPlayerId;
        }

        public async Task EndMatchAsync(Guid matchId)
        {
            await _matchRepository.UpdateMatchStatusAsync(matchId, MatchStatus.Finished);
        }
    }
}
