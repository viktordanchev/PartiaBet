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
                    match.RejoinDeadline = null;

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

        public async Task<double> GetMatchAutoEndTimeRemainingAsync(Guid playerId)
        {
            var matchId = await _cacheService.GetPlayerMatchIdAsync(playerId);

            if (matchId == Guid.Empty)
            {
                return 0;
            }

            var match = await _cacheService.GetMatchAsync(matchId);
            var timeLeft = match.RejoinDeadline - DateTime.UtcNow;

            return timeLeft.Value.TotalSeconds;
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
                    _matchTimer.RemoveTimer(playerId);
                    var player = match.Players.First(p => p.Id == playerId);
                    player.Timer.TimeLeft = player.Timer.TurnExpiresAt - DateTime.UtcNow;

                    var timeLeft = player.Timer.TimeLeft;
                    if (match.GameType != GameType.Chess) 
                    {
                        timeLeft = TimeSpan.FromMinutes(1);
                    }

                    var nextId = SwtichTurnAsync(match, playerId);
                    var nextPlayer = match.Players.First(p => p.Id == nextId);
                    nextPlayer.Timer.TurnExpiresAt = DateTime.UtcNow.AddMinutes(timeLeft.TotalSeconds);

                    _matchTimer.StartTurnTimer(nextPlayer);

                    await _cacheService.SetMatchAsync(match.Id, match);
                }

                return matchResult;
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

            player.TurnOrder = match.Players.Count + 1;
            player.Status = PlayerStatus.Active;
            player.TeamNumber = 1;

            if (match.GameType == GameType.Chess)
            {
                player.Timer.TimeLeft = TimeSpan.FromMinutes(10);
            }
            else
            {
                player.Timer.TimeLeft = TimeSpan.FromMinutes(1);
            }

            match.Players.Add(player);
            await _cacheService.SetPlayerMatchAsync(playerId, match.Id);

            return player;
        }

        private void StartMatch(MatchModel match)
        {
            match.Status = MatchStatus.Ongoing;

            var gameService = _gameFactory.GetGameService(match.GameType);
            match.Board = gameService.CreateGameBoard(match.Players);

            var playerInTurn = match.Players.First(p => p.IsMyTurn);
            _matchTimer.StartTurnTimer(playerInTurn);
        }

        private void RemovePlayer(MatchModel match, Guid playerId)
        {
            var player = match.Players.First(p => p.Id == playerId);
            match.Players.Remove(player);
        }

        private double HandlePlayerLeaveDuringMatch(MatchModel match, Guid playerId)
        {
            var rejoinDeadline = DateTime.UtcNow.AddMinutes(5);
            var rejoinWindow = TimeSpan.FromMinutes(5);

            match.RejoinDeadline = rejoinDeadline;

            _matchTimer.StartMatchCountdown(
                match.GameType,
                match.Id,
                rejoinWindow
            );

            var player = match.Players.First(p => p.Id == playerId);

            match.Status = MatchStatus.Paused;
            player.Status = PlayerStatus.Disconnected;

            return rejoinWindow.TotalSeconds;
        }

        private МакеMoveResult UpdateBoardAsync(MatchModel match, Guid playerId, string moveDataJson)
        {
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

            return МакеMoveResult.Success(moveDataModel, match.GameType);
        }

        private Guid SwtichTurnAsync(MatchModel match, Guid currentPlayerId)
        {
            var gameService = _gameFactory.GetGameService(GameType.Chess);
            var nextPlayerId = gameService.SwitchTurn(currentPlayerId, match.Players);

            var player = match.Players.First(p => p.Id == nextPlayerId);
            player.IsMyTurn = true;

            return nextPlayerId;
        }
    }
}
