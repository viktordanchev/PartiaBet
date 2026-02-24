using AutoMapper;
using Core.Enums;
using Core.Interfaces.Games;
using Core.Interfaces.Services;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using RestAPI.Dtos.Match;

namespace RestAPI.Hubs
{
    public class MatchHub : Hub
    {
        private readonly IMatchService _matchService;
        private readonly IGameProvider _gameProvider;
        private readonly IMatchPlayersManager _matchPlayersManager;
        private readonly IMapper _mapper;
        private readonly IHubContext<MatchHub> _hubContext;

        public MatchHub(IMatchService matchService,
            IGameProvider gameProvider,
            IMatchPlayersManager matchPlayersManager,
            IMapper mapper,
            IHubContext<MatchHub> hubContext)
        {
            _matchService = matchService;
            _gameProvider = gameProvider;
            _matchPlayersManager = matchPlayersManager;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        [Authorize]
        public async Task LeaveActiveMatch()
        {
            var userId = Guid.Parse(Context.User?.FindFirst("Id")?.Value);

            _matchPlayersManager.MarkAsDisconnected(userId);

            _ = Task.Run(async () =>
            {
                await Task.Delay(15000);

                if (_matchPlayersManager.IsStillDisconnected(userId))

                {
                    var result = await _matchService.HandlePlayerDisconnectAsync(userId);

                    await _hubContext.Clients.Group($"{result.MatchId}").SendAsync("RejoinCountdown", userId, result.TimeLeftToRejoin);
                }
            });
        }

        public async Task JoinGameGroup(GameType gameType)
        {
            if (_gameProvider.IsValidGameType(gameType))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"{gameType}");
            }
        }

        public async Task JoinMatchGroup(Guid matchId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"{matchId}");
        }

        [Authorize]
        public async Task<Guid> CreateMatch(AddMatchDto data)
        {
            var playerId = Guid.Parse(Context.User?.FindFirst("Id")?.Value);

            var match = await _matchService.CreateMatchAsync(data.GameType, data.BetAmount);
            var matchStatus = await _matchService.JoinMatchAsync(match.Id, playerId);

            var matchDto = _mapper.Map<MatchDto>(match);
            var playerDto = _mapper.Map<PlayerDto>(matchStatus.AddedPlayer);
            matchDto.Players.Add(playerDto);

            await Groups.AddToGroupAsync(Context.ConnectionId, $"{match.Id}");
            await Clients.Group($"{match.GameType}").SendAsync("ReceiveMatch", matchDto);

            return match.Id;
        }

        [Authorize]
        public async Task JoinMatch(Guid matchId)
        {
            var playerId = Guid.Parse(Context.User?.FindFirst("Id")?.Value);
            var result = await _matchService.JoinMatchAsync(matchId, playerId);

            if (result.IsInvalid) return;

            var playerDto = _mapper.Map<PlayerDto>(result.AddedPlayer);

            await Groups.AddToGroupAsync(Context.ConnectionId, $"{matchId}");
            await Clients.Group($"{result.GameType}").SendAsync("ReceivePlayer", matchId, playerDto);

            if (result.IsStarted)
            {
                await Clients.Group($"{result.GameType}").SendAsync("StartMatch", matchId);
            }
        }

        [Authorize]
        public async Task MakeMove(Guid matchId, string jsonData)
        {
            var playerId = Guid.Parse(Context.User?.FindFirst("Id")?.Value);
            var result = await _matchService.MakeMoveAsync(matchId, playerId, jsonData);

            if (!result.IsValid) return;

            if (result.IsWinningMove)
            {
                await _matchService.EndMatchAsync(matchId);
            }

            await Clients.Group($"{matchId}").SendAsync("ReceiveMove", result.GameBoard, result.NextId, result.Duration);
        }

        [Authorize]
        public async Task LeaveMatchQueue()
        {
            var playerId = Guid.Parse(Context.User.FindFirst("Id").Value);

            var result = await _matchService.LeaveMatchQueueAsync(playerId);

            if (result.IsRemoved)
            {
                await Clients.Group($"{result.GameType}").SendAsync("RemovePlayer", result.MatchId, playerId);
            }
        }

        [Authorize]
        public async Task<Guid> RejoinMatch()
        {
            var userId = Guid.Parse(Context.User.FindFirst("Id").Value);

            _matchPlayersManager.MarkAsConnected(userId);
            var result = await _matchService.PlayerRejoinMatchAsync(userId);

            await Clients.Group($"{result.MatchId}").SendAsync("RejoinPlayer", userId);

            if (result.MatchStatus == MatchStatus.Ongoing)
            {
                await Clients.Group($"{result.MatchId}").SendAsync("MatchResumed");
            }

            return result.MatchId;
        }
    }
}
