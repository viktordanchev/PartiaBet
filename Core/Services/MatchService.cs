using Common.Exceptions;
using Core.Enums;
using Core.Interfaces.Games;
using Core.Interfaces.Infrastructure;
using Core.Interfaces.Services;
using Core.Models.Match;
using Core.Results.Match;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;

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
        private IMatchTurnService _matchTurnService;

        public MatchService(
            IMatchRepository matchRepository,
            IRedisLockService redisLock,
            ICacheService cacheService,
            IGameFactory gameFactory,
            IGameProvider gameProvider,
            IMatchTimer matchTimer,
            IMatchTurnService matchTurnService)
        {
            _matchRepository = matchRepository;
            _redisLock = redisLock;
            _cacheService = cacheService;
            _gameFactory = gameFactory;
            _gameProvider = gameProvider;
            _matchTimer = matchTimer;
            _matchTurnService = matchTurnService;
        }

        public async Task<HandlePlayerDisconnectResult> HandlePlayerDisconnectAsync(Guid playerId)
        {
            var matchId = await _cacheService.GetPlayerMatchIdAsync(playerId);
            var lockHandle = await _redisLock.AcquireAsync($"lock:match:{matchId}");

            if (lockHandle == null)
            {
                throw new TimeoutException("Match is busy. Please try again.");
            }

            try
            {
                var match = await _cacheService.GetMatchAsync(matchId);

                var playerInTurn = match.Players.First(p => p.IsOnTurn);
                playerInTurn.Timer.IsPaused = true;
                var remaining = playerInTurn.Timer.TurnExpiresAt - DateTime.UtcNow;
                remaining += TimeSpan.FromSeconds(5);
                playerInTurn.Timer.TimeLeft = remaining;

                var player = match.Players.First(p => p.Id == playerId);
                player.Status = PlayerStatus.Disconnected;

                var rejoinDeadline = DateTime.UtcNow.AddMinutes(5);
                var rejoinWindow = TimeSpan.FromMinutes(5);
                match.RejoinDeadline = rejoinDeadline;
                match.Status = MatchStatus.Paused;

                _matchTimer.StartMatchCountdown(
                    match.GameType,
                    match.Id,
                    rejoinWindow
                );

                await _cacheService.SetMatchAsync(match.Id, match);

                return HandlePlayerDisconnectResult.Success(match.Id, rejoinWindow.TotalSeconds);
            }
            finally
            {
                await _redisLock.ReleaseAsync(lockHandle);
            }
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

        public async Task<JoinMatchResult> JoinMatchAsync(Guid matchId, Guid playerId)
        {
            var lockHandle = await _redisLock.AcquireAsync($"lock:match:{matchId}");

            if (lockHandle == null)
            {
                throw new TimeoutException("Match is busy. Please try again.");
            }

            try
            {
                var match = await _cacheService.GetMatchAsync(matchId);
                var maxPlayersCount = _gameProvider.GetMaxPlayersCount(match.GameType);

                if (match.Status == MatchStatus.Ongoing ||
                    match.Players.Any(p => p.Id == playerId) ||
                    match.Players.Count >= maxPlayersCount)
                {
                    return JoinMatchResult.Invalid();
                }

                var addedPlayer = await AddPlayerAsync(match, playerId);

                if (match.Players.Count == maxPlayersCount)
                {
                    StartMatch(match);
                }

                await _cacheService.SetMatchAsync(match.Id, match);

                return JoinMatchResult.Success(
                    addedPlayer,
                    match.GameType,
                    match.Status == MatchStatus.Ongoing);
            }
            finally
            {
                await _redisLock.ReleaseAsync(lockHandle);
            }
        }

        public async Task<LeaveMatchQueueResult> LeaveMatchQueueAsync(Guid playerId)
        {
            var matchId = await _cacheService.GetPlayerMatchIdAsync(playerId);
            var lockHandle = await _redisLock.AcquireAsync($"lock:match:{matchId}");

            if (lockHandle == null)
            {
                throw new TimeoutException("Match is busy. Please try again.");
            }

            try
            {
                var isRemoved = false;
                var match = await _cacheService.GetMatchAsync(matchId);

                if (match.Status == MatchStatus.Created)
                {
                    isRemoved = true;

                    var player = match.Players.First(p => p.Id == playerId);
                    match.Players.Remove(player);

                    if (match.Players.Count == 0)
                    {
                        await _cacheService.RemoveMatchAsync(match.Id, match.GameType);
                    }

                    await _cacheService.SetMatchAsync(match.Id, match);
                }

                return LeaveMatchQueueResult.Success(match.Id, isRemoved, match.GameType);
            }
            finally
            {
                await _redisLock.ReleaseAsync(lockHandle);
            }
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

        public async Task<PlayerRejoinMatchResult> PlayerRejoinMatchAsync(Guid playerId)
        {
            var matchId = await _cacheService.GetPlayerMatchIdAsync(playerId);
            var lockHandle = await _redisLock.AcquireAsync($"lock:match:{matchId}");

            if (lockHandle == null)
            {
                throw new TimeoutException("Match is busy. Please try again.");
            }

            try
            {
                var match = await _cacheService.GetMatchAsync(matchId);
                var player = match.Players.First(p => p.Id == playerId);

                player.Status = PlayerStatus.Active;

                if (match.Players.All(p => p.Status == PlayerStatus.Active))
                {
                    match.Status = MatchStatus.Ongoing;
                    match.RejoinDeadline = null;

                    _matchTimer.RemoveTimer(matchId);

                    var playerInTurn = match.Players.First(p => p.IsOnTurn);
                    playerInTurn.Timer.IsPaused = false;
                    playerInTurn.Timer.TurnExpiresAt = DateTime.UtcNow.Add(playerInTurn.Timer.TimeLeft);

                    _matchTimer.StartTurnTimer(playerInTurn);
                }

                await _cacheService.SetMatchAsync(match.Id, match);

                return PlayerRejoinMatchResult.Success(match.Id, match.Status);
            }
            finally
            {
                await _redisLock.ReleaseAsync(lockHandle);
            }
        }

        public async Task<HandlePlayerDisconnectResult> GetMatchCountdownAsync(Guid playerId)
        {
            var matchId = await _cacheService.GetPlayerMatchIdAsync(playerId);

            if (matchId == Guid.Empty)
                return HandlePlayerDisconnectResult.Success(matchId, 0);

            var match = await _cacheService.GetMatchAsync(matchId);
            double timeLeft = 0;

            if (match.RejoinDeadline != null)
            {
                timeLeft = (match.RejoinDeadline.Value - DateTime.UtcNow).TotalSeconds;
            }

            return HandlePlayerDisconnectResult.Success(matchId, timeLeft);
        }

        public async Task<МакеMoveResult> MakeMoveAsync(Guid matchId, Guid playerId, string moveDataJson)
        {
            var lockHandle = await _redisLock.AcquireAsync($"lock:match:{matchId}");

            if (lockHandle == null)
            {
                throw new TimeoutException("Match is busy. Please try again.");
            }

            try
            {
                var match = await _cacheService.GetMatchAsync(matchId);
                var matchResult = UpdateBoardAsync(match, playerId, moveDataJson);

                if (matchResult.IsValid)
                {
                    var (nextPlayer, timeLeft) = SwtichTurnAsync(match, playerId);

                    matchResult.NextId = nextPlayer;
                    matchResult.Duration = timeLeft;

                    await _cacheService.SetMatchAsync(match.Id, match);
                }

                return matchResult;
            }
            finally
            {
                await _redisLock.ReleaseAsync(lockHandle);
            }
        }

        public async Task EndMatchAsync(Guid matchId)
        {
            var lockHandle = await _redisLock.AcquireAsync($"lock:match:{matchId}");

            if (lockHandle == null)
            {
                throw new TimeoutException("Match is busy. Please try again.");
            }

            try
            {
                var match = await _cacheService.GetMatchAsync(matchId); 

                await _cacheService.RemoveMatchAsync(matchId, match.GameType);
            }
            finally
            {
                await _redisLock.ReleaseAsync(lockHandle);
            }
        }

        //private methods

        private async Task<PlayerModel> AddPlayerAsync(MatchModel match, Guid playerId)
        {
            var player = await _matchRepository.GetPlayerDataAsync(playerId);

            _matchTurnService.SetTimeLeft(match.GameType, player);
            player.TurnOrder = match.Players.Count + 1;
            player.Status = PlayerStatus.Active;
            player.TeamNumber = 1;

            match.Players.Add(player);
            await _cacheService.SetPlayerMatchAsync(playerId, match.Id);

            return player;
        }

        private void StartMatch(MatchModel match)
        {
            match.Status = MatchStatus.Ongoing;

            var gameService = _gameFactory.GetGameService(match.GameType);
            match.Board = gameService.CreateGameBoard(match.Players);

            var playerInTurn = match.Players.First(p => p.IsOnTurn);
            _matchTurnService.StartTurn(match, playerInTurn);
        }

        private МакеMoveResult UpdateBoardAsync(MatchModel match, Guid playerId, string moveDataJson)
        {
            var gameService = _gameFactory.GetGameService(match.GameType);
            var moveDataModel = _gameFactory.GetMakeMoveDto(match.GameType, moveDataJson);

            if (!gameService.IsValidMove(match.Board, moveDataModel))
            {
                return МакеMoveResult.Invalid();
            }

            gameService.UpdateBoard(match.Board, moveDataModel);

            if (gameService.IsWinningMove(match.Board))
            {
                return МакеMoveResult.Win(playerId);
            }

            return МакеMoveResult.Success(match.Board, match.GameType);
        }

        private (Guid, double) SwtichTurnAsync(MatchModel match, Guid currentPlayerId)
        {
            var gameService = _gameFactory.GetGameService(GameType.Chess);

            var currPlayer = match.Players.First(p => p.Id == currentPlayerId);
            _matchTurnService.EndTurn(match, currPlayer);

            var nextPlayerId = gameService.SwitchTurn(currentPlayerId, match.Players);

            var nextPlayer = match.Players.First(p => p.Id == nextPlayerId);
            _matchTurnService.StartTurn(match, nextPlayer);

            nextPlayer.IsOnTurn = true;

            return (nextPlayerId, nextPlayer.Timer.TimeLeft.TotalSeconds);
        }
    }
}
