using Core.Enums;
using Core.Interfaces.Games;
using Core.Interfaces.Infrastructure;
using Core.Interfaces.Services;
using Core.Models.Match;
using Core.Results.Match;
using System.Data;
using static Common.Constants.Constants;

namespace Core.Services
{
    public class MatchService : IMatchService
    {
        private IMatchRepository _matchRepository;
        private IMatchCache _matchCache;
        private IRedisLockService _redisLock;
        private IGameFactory _gameFactory;
        private IGameProvider _gameProvider;
        private IMatchTurnManager _matchTurnManager;
        private IRatingCalculator _ratingCalculator;

        public MatchService(
            IMatchRepository matchRepository,
            IRedisLockService redisLock,
            IMatchCache matchCache,
            IGameFactory gameFactory,
            IGameProvider gameProvider,
            IMatchTurnManager matchTurnManager,
             IRatingCalculator ratingCalculator)
        {
            _matchRepository = matchRepository;
            _redisLock = redisLock;
            _matchCache = matchCache;
            _gameFactory = gameFactory;
            _gameProvider = gameProvider;
            _matchTurnManager = matchTurnManager;
            _ratingCalculator = ratingCalculator;
        }

        public async Task<HandlePlayerDisconnectResult> HandlePlayerDisconnectAsync(Guid playerId)
        {
            var matchId = await _matchCache.GetPlayerMatchIdAsync(playerId);
            var lockHandle = await _redisLock.AcquireAsync($"lock:match:{matchId}");

            if (lockHandle == null)
            {
                throw new TimeoutException("Match is busy. Please try again.");
            }

            try
            {
                var match = await _matchCache.GetMatchAsync(matchId);

                var playerInTurn = match.Players.First(p => p.IsOnTurn);
                var remaining = playerInTurn.Timer.TurnExpiresAt - DateTime.UtcNow;
                remaining += TimeSpan.FromSeconds(5);
                playerInTurn.Timer.TimeLeft = remaining;

                var player = match.Players.First(p => p.Id == playerId);
                player.Status = PlayerStatus.Disconnected;
                player.Timer.IsPaused = true;

                _matchTurnManager.StartDisconnectCountdown(match.Id);

                match.RejoinDeadline = DateTime.Now.AddSeconds(MatchEndCountdownSeconds);
                match.Status = MatchStatus.Paused;

                await _matchCache.SetMatchAsync(match.Id, match);

                return HandlePlayerDisconnectResult.Success(match.Id, MatchEndCountdownSeconds);
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

            await _matchCache.SetMatchAsync(match.Id, match);

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
                var match = await _matchCache.GetMatchAsync(matchId);
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

                await _matchCache.SetMatchAsync(match.Id, match);

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
            var matchId = await _matchCache.GetPlayerMatchIdAsync(playerId);
            var lockHandle = await _redisLock.AcquireAsync($"lock:match:{matchId}");

            if (lockHandle == null)
            {
                throw new TimeoutException("Match is busy. Please try again.");
            }

            try
            {
                var isRemoved = false;
                var match = await _matchCache.GetMatchAsync(matchId);

                if (match.Status == MatchStatus.Created)
                {
                    isRemoved = true;

                    var player = match.Players.First(p => p.Id == playerId);
                    match.Players.Remove(player);

                    await _matchCache.SetMatchAsync(match.Id, match);

                    if (match.Players.Count == 0)
                    {
                        await _matchCache.RemoveMatchAsync(match);
                    }
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
            var matches = await _matchCache.GetAllMatchesAsync(gameType);

            return matches;
        }

        public async Task<MatchModel?> GetMatchAsync(Guid matchId)
        {
            var match = await _matchCache.GetMatchAsync(matchId);

            return match;
        }

        public async Task<PlayerRejoinMatchResult> PlayerRejoinMatchAsync(Guid playerId)
        {
            var matchId = await _matchCache.GetPlayerMatchIdAsync(playerId);
            var lockHandle = await _redisLock.AcquireAsync($"lock:match:{matchId}");

            if (lockHandle == null)
            {
                throw new TimeoutException("Match is busy. Please try again.");
            }

            try
            {
                var match = await _matchCache.GetMatchAsync(matchId);
                var player = match.Players.First(p => p.Id == playerId);

                player.Status = PlayerStatus.Active;
                player.Timer.IsPaused = false;

                if (match.Players.All(p => p.Status == PlayerStatus.Active))
                {
                    match.Status = MatchStatus.Ongoing;
                    match.RejoinDeadline = null;

                    _matchTurnManager.RemoveTimer(matchId);

                    var playerInTurn = match.Players.First(p => p.IsOnTurn);
                    playerInTurn.Timer.TurnExpiresAt = DateTime.UtcNow.Add(playerInTurn.Timer.TimeLeft);

                    _matchTurnManager.StartTurn(match, playerInTurn);
                }

                await _matchCache.SetMatchAsync(match.Id, match);

                return PlayerRejoinMatchResult.Success(match.Id, match.Status);
            }
            finally
            {
                await _redisLock.ReleaseAsync(lockHandle);
            }
        }

        public async Task<HandlePlayerDisconnectResult> GetMatchCountdownAsync(Guid playerId)
        {
            var matchId = await _matchCache.GetPlayerMatchIdAsync(playerId);

            if (matchId == Guid.Empty)
                return HandlePlayerDisconnectResult.Success(matchId, 0);

            var match = await _matchCache.GetMatchAsync(matchId);
            double timeLeft = 0;

            if (match.RejoinDeadline != null)
            {
                timeLeft = (match.RejoinDeadline.Value - DateTime.UtcNow).TotalSeconds;
            }

            return HandlePlayerDisconnectResult.Success(matchId, timeLeft);
        }

        public async Task<MakeMoveResult> MakeMoveAsync(Guid matchId, Guid playerId, string moveDataJson)
        {
            var lockHandle = await _redisLock.AcquireAsync($"lock:match:{matchId}");

            if (lockHandle == null)
                throw new TimeoutException("Match is busy. Please try again.");

            try
            {
                var match = await _matchCache.GetMatchAsync(matchId);
                var result = ProcessMove(match, playerId, moveDataJson);
                 
                if (result.Status == MoveStatus.Success || result.Status == MoveStatus.Win)
                {
                    var (nextPlayer, timeLeft) = SwitchTurnAsync(match, playerId);

                    result.NextPlayerId = nextPlayer;
                    result.Duration = timeLeft;

                    await _matchCache.SetMatchAsync(match.Id, match);
                }

                return result;
            }
            finally
            {
                await _redisLock.ReleaseAsync(lockHandle);
            }
        }

        public async Task<EndMatchResult> EndMatchAsync(Guid matchId)
        {
            var lockHandle = await _redisLock.AcquireAsync($"lock:match:{matchId}");

            if (lockHandle == null)
                throw new TimeoutException("Match is busy. Please try again.");

            try
            {
                var match = await _matchCache.GetMatchAsync(matchId);

                await _matchCache.RemoveMatchAsync(match);

                return EndMatchResult.Success(match.Id, match.Players);
            }
            finally
            {
                await _redisLock.ReleaseAsync(lockHandle);
            }
        }

        public async Task SetMatchDrawAsync(Guid matchId)
        {
            var lockHandle = await _redisLock.AcquireAsync($"lock:match:{matchId}");

            if (lockHandle == null)
                throw new TimeoutException("Match is busy. Please try again.");

            try
            {
                var match = await _matchCache.GetMatchAsync(matchId);

                foreach (var player in match.Players)
                {
                    player.NewRating = player.CurrentRating;
                    player.Result = MatchResult.Draw;
                }

                await _matchCache.SetMatchAsync(match.Id, match);
            }
            finally
            {
                await _redisLock.ReleaseAsync(lockHandle);
            }
        }
         
        public (Guid nextPlayerId, double timeLeft) SwitchTurnAsync(MatchModel match, Guid currentPlayerId)
        {
            var gameService = _gameFactory.GetGameService(match.GameType);

            var currPlayer = match.Players.First(p => p.Id == currentPlayerId);
            _matchTurnManager.EndTurn(match.GameType, currPlayer);
            currPlayer.IsOnTurn = false;

            var nextPlayerId = gameService.SwitchTurn(currentPlayerId, match.Players);
            var nextPlayer = match.Players.First(p => p.Id == nextPlayerId);
            nextPlayer.IsOnTurn = true;
            _matchTurnManager.StartTurn(match, nextPlayer);

            return (nextPlayerId, nextPlayer.Timer.TimeLeft.TotalSeconds);
        }

        //private methods

        private async Task<PlayerModel> AddPlayerAsync(MatchModel match, Guid playerId)
        {
            var player = await _matchRepository.GetPlayerDataAsync(playerId);
            player.Status = PlayerStatus.Active;

            match.Players.Add(player);
            await _matchCache.SetPlayerMatchAsync(playerId, match.Id);

            return player;
        }

        private void StartMatch(MatchModel match)
        {
            match.Status = MatchStatus.Ongoing;

            var gameService = _gameFactory.GetGameService(match.GameType);
            match.Board = gameService.CreateGameBoard(match.Players);

            var playerInTurn = match.Players.First(p => p.IsOnTurn);
            _matchTurnManager.StartTurn(match, playerInTurn);
        }

        private MakeMoveResult ProcessMove(MatchModel match, Guid winnerId, string moveDataJson)
        {
            var gameService = _gameFactory.GetGameService(match.GameType);
            var moveData = _gameFactory.GetMakeMoveDto(match.GameType, moveDataJson);

            if (!gameService.IsValidMove(match.Board, moveData))
            {
                return MakeMoveResult.Invalid();
            }

            gameService.UpdateBoard(match.Board, moveData);

            var moveStatus = MoveStatus.Success;

            if (gameService.IsWinningMove(match.Board, moveData))
            {
                gameService.SetMatchResults(match.Players, winnerId);

                _ratingCalculator.CalculatePlayersRating(match);

                moveStatus = MoveStatus.Win;
            }

            return MakeMoveResult.Success(match.Board, moveStatus);
        }
    }
}
