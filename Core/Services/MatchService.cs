using Common.Exceptions;
using Core.Enums;
using Core.Interfaces.Games;
using Core.Interfaces.Infrastructure;
using Core.Interfaces.Services;
using Core.Models.Match;
using Core.Results.Match;
using Microsoft.AspNetCore.Http;

namespace Core.Services
{
    public class MatchService : IMatchService
    {
        private IMatchRepository _matchRepository;
        private ICacheService _cacheService;
        private IRedisLockService _redisLock;
        private IGameFactory _gameFactory;
        private IGameProvider _gameProvider;
        private IMatchTimer _matchTimer;

        public MatchService(
            IMatchRepository matchRepository,
            IRedisLockService redisLock,
            ICacheService cacheService,
            IGameFactory gameFactory,
            IGameProvider gameProvider,
            IMatchTimer matchTimer)
        {
            _matchRepository = matchRepository;
            _redisLock = redisLock;
            _cacheService = cacheService;
            _gameFactory = gameFactory;
            _gameProvider = gameProvider;
            _matchTimer = matchTimer;
        }

        public async Task<MatchModel> CreateMatchAsync(GameType gameType, decimal betAmount)
        {
            var match = new MatchModel()
            {
                BetAmount = betAmount,
                GameType = gameType,
                Status = MatchStatus.Created,
                MaxPlayersCount = _gameProvider.GetMaxPlayersCount(gameType),
            };

            await _cacheService.SetMatchAsync(match.Id, match);

            return match;
        }

        public async Task<AddPlayerResult> AddPlayerAsync(Guid matchId, Guid playerId)
        {
            var lockHandle = await _redisLock.AcquireAsync($"lock:match:{matchId}");

            if (lockHandle == null)
            {
                throw new TimeoutException("Match is busy. Please try again.");
            }

            try
            {
                var match = await _cacheService.GetMatchAsync(matchId);

                if (match.Status == MatchStatus.Ongoing ||
                    match.Players.Any(p => p.Id == playerId))
                {
                    throw new InvalidOperationException("Cannot join an ongoing match.");
                }

                var gameService = _gameFactory.GetGameService(match.GameType);
                var maxPlayersCount = _gameProvider.GetMaxPlayersCount(match.GameType);

                if (match.Players.Count >= maxPlayersCount)
                {
                    throw new InvalidOperationException("Match is full.");
                }

                var player = await _matchRepository.GetPlayerDataAsync(playerId);
                player.TurnOrder = match.Players.Count + 1;
                player.Status = PlayerStatus.Active;
                player.TeamNumber = 1;

                match.Players.Add(player);
                await _cacheService.SetPlayerMatchAsync(playerId, matchId);

                if (match.Players.Count == maxPlayersCount)
                {
                    match.Status = MatchStatus.Ongoing;
                    match.Board = gameService.CreateGameBoard(match.Players);
                }

                await _cacheService.SetMatchAsync(match.Id, match);

                return AddPlayerResult.Success(
                    player,
                    match.GameType,
                    match.Status == MatchStatus.Ongoing);
            }
            finally
            {
                await _redisLock.ReleaseAsync(lockHandle);
            }
        }

        public async Task<LeaveMatchResult> LeaveMatchAsync(Guid matchId, Guid playerId)
        {
            var lockHandle = await _redisLock.AcquireAsync($"lock:match:{matchId}");

            if (lockHandle == null)
            {
                throw new TimeoutException("Match is busy. Please try again.");
            }

            try
            {
                var isRemoved = false;
                double timeLeft = 0;
                var match = await _cacheService.GetMatchAsync(matchId);

                if (match.Status == MatchStatus.Ongoing)
                {
                    timeLeft = HandlePlayerLeaveDuringMatch(match, playerId);
                }
                else
                {
                    RemovePlayer(match, playerId);
                    isRemoved = true;
                }

                await _cacheService.SetMatchAsync(match.Id, match);

                return LeaveMatchResult.Success(isRemoved, match.GameType, timeLeft);
            }
            finally
            {
                await _redisLock.ReleaseAsync(lockHandle);
            }
        }

        public async Task<МакеMoveResult> UpdateBoardAsync(Guid matchId, Guid playerId, string moveDataJson)
        {
            var match = await _cacheService.GetMatchAsync(matchId);

            if (match == null) return МакеMoveResult.Invalid();

            var gameService = _gameFactory.GetGameService(match.GameType);
            var moveDataModel = _gameFactory.GetMakeMoveDto(match.GameType, moveDataJson);
            var isValidMove = gameService.IsValidMove(match.Board, moveDataModel);

            if (!isValidMove)
            {
                return МакеMoveResult.Invalid();
            }
            else if (gameService.IsWinningMove(match.Board))
            {
                return МакеMoveResult.Win(playerId);
            }

            gameService.UpdateBoard(match.Board, moveDataModel);
            await _cacheService.SetMatchAsync(match.Id, match);

            return МакеMoveResult.Success(moveDataModel, match.GameType);
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

        public async Task<RejoinMatchResult> RejoinMatchAsync(Guid matchId, Guid playerId)
        {
            var lockHandle = await _redisLock.AcquireAsync($"lock:match:{matchId}");

            if (lockHandle == null)
            {
                throw new TimeoutException("Match is busy. Please try again.");
            }

            try
            {
                var match = await _cacheService.GetMatchAsync(matchId);
                var player = match.Players.First(p => p.Id == playerId);
                var hasChanged = false;

                player.Status = PlayerStatus.Active;

                if (match.Players.All(p => p.Status == PlayerStatus.Active))
                {
                    hasChanged = true;
                    match.Status = MatchStatus.Ongoing;
                    _matchTimer.RemoveTimer(matchId);
                }

                await _cacheService.SetMatchAsync(match.Id, match);

                return RejoinMatchResult.Success(hasChanged, match.Status);
            }
            finally
            {
                await _redisLock.ReleaseAsync(lockHandle);
            }
        }

        public async Task<double> GetPlayerRejoinTimeAsync(Guid playerId)
        {
            var matchId = await _cacheService.GetPlayerMatchIdAsync(playerId);

            if (matchId == Guid.Empty)
            {
                return 0;
            }

            return _matchTimer.GetActiveTimer(matchId);
        }

        //private methods

        private void RemovePlayer(MatchModel match, Guid playerId)
        {
            var player = match.Players.First(p => p.Id == playerId);
            match.Players.Remove(player);
        }

        private double HandlePlayerLeaveDuringMatch(MatchModel match, Guid playerId)
        {
            var timeLeft = _matchTimer.StartMatchCountdown(match.GameType, match.Id);
            var player = match.Players.First(p => p.Id == playerId);

            match.Status = MatchStatus.Paused;
            player.Status = PlayerStatus.Disconnected;

            return timeLeft;
        }
    }
}
