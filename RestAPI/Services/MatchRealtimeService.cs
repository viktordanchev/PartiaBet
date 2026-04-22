using AutoMapper;
using Core.Enums;
using Core.Interfaces.Games;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.SignalR;
using RestAPI.Dtos.Match;
using RestAPI.Hubs;
using RestAPI.Services.Interfaces;

namespace RestAPI.Services
{
    public class MatchRealtimeService : IMatchRealtimeService
    {
        private readonly IMatchService _matchService;
        private readonly IGameProvider _gameProvider;
        private readonly IUserConnectionTracker _userConnectionTracker;
        private readonly IMapper _mapper;
        private readonly IHubContext<AppHub> _hub;

        public MatchRealtimeService(
            IMatchService matchService,
            IGameProvider gameProvider,
            IUserConnectionTracker userConnectionTracker,
            IMapper mapper,
            IHubContext<AppHub> hub)
        {
            _matchService = matchService;
            _gameProvider = gameProvider;
            _userConnectionTracker = userConnectionTracker;
            _mapper = mapper;
            _hub = hub;
        }

        public async Task OnConnectedAsync(Guid userId, string connectionId)
        {
            _userConnectionTracker.AddConnection(userId, "match", connectionId);

            await Task.CompletedTask;
        }

        public async Task OnDisconnectedAsync(Guid userId, string connectionId)
        {
            await Task.Delay(TimeSpan.FromSeconds(5));

            _userConnectionTracker.RemoveConnection(userId, "match", connectionId);

            if (_userConnectionTracker.HasConnections(userId, "match"))
                return;

            var result = await _matchService.HandlePlayerDisconnectAsync(userId);

            if (result.IsSuccess)
            {
                await _hub.Clients
                    .Group(result.MatchId.ToString())
                    .SendAsync("RejoinCountdown", result.RejoinDeadline);
            }
        }

        public async Task JoinGameGroupAsync(string connectionId, GameType gameType)
        {
            if (_gameProvider.IsValidGameType(gameType))
            {
                await _hub.Groups.AddToGroupAsync(connectionId, gameType.ToString());
            }
        }

        public async Task JoinMatchGroupAsync(string connectionId, Guid matchId)
        {
            await _hub.Groups.AddToGroupAsync(connectionId, matchId.ToString());
        }

        public async Task<Guid> CreateMatchAsync(Guid userId, string connectionId, AddMatchDto data)
        {
            var match = await _matchService.CreateMatchAsync(data.GameType, data.BetAmount);
            var matchStatus = await _matchService.JoinMatchAsync(match.Id, userId);

            var matchDto = _mapper.Map<MatchDto>(match);
            var playerDto = _mapper.Map<PlayerDto>(matchStatus.AddedPlayer);

            matchDto.Players.Add(playerDto);

            await _hub.Groups.AddToGroupAsync(connectionId, match.Id.ToString());
            await _hub.Clients.Group(match.GameType.ToString())
                .SendAsync("ReceiveMatch", matchDto);

            return match.Id;
        }

        public async Task JoinMatchAsync(Guid userId, string connectionId, Guid matchId)
        {
            var result = await _matchService.JoinMatchAsync(matchId, userId);

            if (result.IsInvalid) return;

            var playerDto = _mapper.Map<PlayerDto>(result.AddedPlayer);

            await _hub.Groups.AddToGroupAsync(connectionId, matchId.ToString());

            await _hub.Clients.Group(result.GameType.ToString())
                .SendAsync("ReceivePlayer", matchId, playerDto);

            if (result.IsStarted)
            {
                await _hub.Clients.Group(result.GameType.ToString())
                    .SendAsync("StartMatch", matchId);
            }
        }

        public async Task MakeMoveAsync(Guid userId, Guid matchId, string jsonData)
        {
            var result = await _matchService.MakeMoveAsync(matchId, userId, jsonData);
            if (result.Status == MoveStatus.Invalid)
                return;

            await _hub.Clients.Group(matchId.ToString())
                .SendAsync("ReceiveMove", result.GameBoard, result.NextPlayerId, result.Duration);

            if (result.Status == MoveStatus.Win)
            {
                var end = await _matchService.EndMatchAsync(matchId);

                var players = _mapper.Map<List<PlayerMatchStatsDto>>(end.Players);

                await _hub.Clients.Group(matchId.ToString())
                    .SendAsync("EndMatch", players);
            }
        }

        public async Task LeaveMatchQueueAsync(Guid userId)
        {
            var result = await _matchService.LeaveMatchQueueAsync(userId);

            if (result.IsRemoved)
            {
                await _hub.Clients.Group(result.GameType.ToString())
                    .SendAsync("RemovePlayer", result.MatchId, userId);
            }
        }

        public async Task RejoinMatchAsync(Guid userId, string connectionId)
        {
            _userConnectionTracker.AddConnection(userId, "match", connectionId);

            var result = await _matchService.PlayerRejoinMatchAsync(userId);

            if (result.MatchStatus == MatchStatus.Ongoing)
            {
                await _hub.Clients.Group(result.MatchId.ToString())
                    .SendAsync("MatchResumed");
            }
        }
    }
}
