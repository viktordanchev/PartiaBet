using Common.Exceptions;
using Core.Enums;
using Core.Interfaces.Games;
using Core.Interfaces.Infrastructure;
using Core.Interfaces.Services;
using Core.Models.Games;
using Core.Models.Match;
using Microsoft.AspNetCore.Http;

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

        public async Task<MatchModel> CreateMatchAsync(GameType gameType, decimal betAmount)
        {
            var match = new MatchModel()
            {
                BetAmount = betAmount,
                GameType = gameType,
                MatchStatus = MatchStatus.Created,
                MaxPlayersCount = _gameProvider.GetMaxPlayersCount(gameType),
            };

            await _cacheService.SetMatchAsync(match.Id, match);

            return match;
        }

        public async Task<PlayerModel> AddPlayerAsync(Guid matchId, Guid playerId)
        {
            var match = await _cacheService.GetMatchAsync(matchId);

            if (match.MatchStatus == MatchStatus.Ongoing || match.Players.Any(p => p.Id == playerId))
            {
                throw new InvalidOperationException("Cannot join an ongoing match.");
            }

            var gameService = _gameFactory.GetGameService(match.GameType);
            var maxPlayersCount = _gameProvider.GetMaxPlayersCount(match.GameType);

            var player = await _matchRepository.GetPlayerDataAsync(playerId);
            player.TurnOrder = match.Players.Count + 1;
            player.Status = PlayerStatus.Active;
            player.TeamNumber = 1;
            
            match.Players.Add(player);

            await _cacheService.SetMatchAsync(match.Id, match);

            return player;
        }

        public async Task<MoveResultModel> UpdateBoardAsync(Guid matchId, Guid playerId, string moveDataJson)
        {
            var match = await _cacheService.GetMatchAsync(matchId);

            if (match == null) return MoveResultModel.Invalid();

            var gameService = _gameFactory.GetGameService(match.GameType);
            var moveDataModel = _gameFactory.GetMakeMoveDto(match.GameType, moveDataJson);
            var isValidMove = gameService.IsValidMove(match.Board, moveDataModel);

            if (!isValidMove)
            {
                return MoveResultModel.Invalid();
            }
            else if (gameService.IsWinningMove(match.Board))
            {
                return MoveResultModel.Win(playerId);
            }

            gameService.UpdateBoard(match.Board, moveDataModel);
            await _cacheService.SetMatchAsync(match.Id, match);

            return MoveResultModel.Success(moveDataModel, match.GameType);
        }

        public async Task<Guid> SwtichTurnAsync(Guid matchId, Guid currentPlayerId)
        {
            var match = await _cacheService.GetMatchAsync(matchId); 
            var gameService = _gameFactory.GetGameService(GameType.Chess);
            var nextPlayerId = gameService.SwitchTurn(currentPlayerId, match.Players);

            return nextPlayerId;
        }

        public async Task<IEnumerable<MatchModel>> GetActiveMatchesAsync(GameType gameType)
        {
            var matches = await _cacheService.GetAllMatchesAsync(gameType);

            return matches;
        }

        public async Task<MatchModel> GetMatchAsync(Guid matchId)
        {
            var match = await _cacheService.GetMatchAsync(matchId);

            if (match == null)
            {
                throw new ApiException(statusCode: StatusCodes.Status404NotFound);
            }

            return match;
        }
    }
}
