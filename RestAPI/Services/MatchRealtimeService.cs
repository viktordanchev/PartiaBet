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
        private readonly IHubContext<MatchHub> _hub;

        public MatchRealtimeService(
            IMatchService matchService,
            IGameProvider gameProvider,
            IUserConnectionTracker userConnectionTracker,
            IMapper mapper,
            IHubContext<MatchHub> hub)
        {
            _matchService = matchService;
            _gameProvider = gameProvider;
            _userConnectionTracker = userConnectionTracker;
            _mapper = mapper;
            _hub = hub;
        }

        public Task OnConnected(HubCallerContext context)
        {
            var userId = GetUserId(context);
            _userConnectionTracker.AddConnection(userId, "match", context.ConnectionId);

            return Task.CompletedTask;
        }

        public async Task OnDisconnected(HubCallerContext context, Exception? ex)
        {
            var userId = GetUserId(context);

            await Task.Delay(TimeSpan.FromSeconds(5));

            _userConnectionTracker.RemoveConnection(userId, "match", context.ConnectionId);

            if (_userConnectionTracker.HasConnections(userId, "match"))
                return;

            var result = await _matchService.HandlePlayerDisconnectAsync(userId);

            if (result.IsSuccess)
            {
                await _hub.Clients
                    .Group(result.MatchId.ToString())
                    .SendAsync("RejoinCountdown", userId, result.TimeLeftToRejoin);
            }
        }

        public async Task JoinGameGroup(string connectionId, GameType gameType)
        {
            if (_gameProvider.IsValidGameType(gameType))
            {
                await _hub.Groups.AddToGroupAsync(connectionId, gameType.ToString());
            }
        }

        public async Task JoinMatchGroup(string connectionId, Guid matchId)
        {
            await _hub.Groups.AddToGroupAsync(connectionId, matchId.ToString());
        }

        public async Task<Guid> CreateMatch(HubCallerContext context, AddMatchDto data)
        {
            var playerId = GetUserId(context);

            var match = await _matchService.CreateMatchAsync(data.GameType, data.BetAmount);
            var matchStatus = await _matchService.JoinMatchAsync(match.Id, playerId);

            var matchDto = _mapper.Map<MatchDto>(match);
            var playerDto = _mapper.Map<PlayerDto>(matchStatus.AddedPlayer);

            matchDto.Players.Add(playerDto);

            await _hub.Groups.AddToGroupAsync(context.ConnectionId, match.Id.ToString());
            await _hub.Clients.Group(match.GameType.ToString())
                .SendAsync("ReceiveMatch", matchDto);

            return match.Id;
        }

        public async Task JoinMatch(HubCallerContext context, Guid matchId)
        {
            var playerId = GetUserId(context);

            var result = await _matchService.JoinMatchAsync(matchId, playerId);

            if (result.IsInvalid) return;

            var playerDto = _mapper.Map<PlayerDto>(result.AddedPlayer);

            await _hub.Groups.AddToGroupAsync(context.ConnectionId, matchId.ToString());

            await _hub.Clients.Group(result.GameType.ToString())
                .SendAsync("ReceivePlayer", matchId, playerDto);

            if (result.IsStarted)
            {
                await _hub.Clients.Group(result.GameType.ToString())
                    .SendAsync("StartMatch", matchId);
            }
        }

        public async Task MakeMove(HubCallerContext context, Guid matchId, string jsonData)
        {
            var playerId = GetUserId(context);

            var result = await _matchService.MakeMoveAsync(matchId, playerId, jsonData);

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

        public async Task LeaveMatchQueue(HubCallerContext context)
        {
            var playerId = GetUserId(context);

            var result = await _matchService.LeaveMatchQueueAsync(playerId);

            if (result.IsRemoved)
            {
                await _hub.Clients.Group(result.GameType.ToString())
                    .SendAsync("RemovePlayer", result.MatchId, playerId);
            }
        }

        public async Task RejoinMatch(HubCallerContext context)
        {
            var userId = GetUserId(context);

            _userConnectionTracker.AddConnection(userId, "match", context.ConnectionId);

            var result = await _matchService.PlayerRejoinMatchAsync(userId);

            if (result.MatchStatus == MatchStatus.Ongoing)
            {
                await _hub.Clients.Group(result.MatchId.ToString())
                    .SendAsync("MatchResumed");
            }
        }

        //private methods

        private Guid GetUserId(HubCallerContext context)
        {
            return Guid.Parse(context.User?.FindFirst("Id")?.Value!);
        }
    }
}
