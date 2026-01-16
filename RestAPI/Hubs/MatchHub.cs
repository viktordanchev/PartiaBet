using AutoMapper;
using Core.Enums;
using Core.Interfaces.Games;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using RestAPI.Dtos.Match;

namespace RestAPI.Hubs
{
    public class MatchHub : Hub
    {
        private readonly IMatchService _matchService;
        private readonly IGameProvider _gameProvider;
        private readonly IMatchTimer _matchTimer;
        private readonly IMapper _mapper;

        public MatchHub(IMatchService matchService,
            IGameProvider gameProvider,
            IMatchTimer matchTimer, 
            IMapper mapper)
        {
            _matchService = matchService;
            _gameProvider = gameProvider;
            _matchTimer = matchTimer;
            _mapper = mapper;
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
            var matchStatus = await _matchService.AddPlayerAsync(match.Id, playerId);

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
            var matchStatus = await _matchService.AddPlayerAsync(matchId, playerId);
            var playerDto = _mapper.Map<PlayerDto>(matchStatus.AddedPlayer);

            await Groups.AddToGroupAsync(Context.ConnectionId, $"{matchId}");
            await Clients.Group($"{matchStatus.GameType}").SendAsync("ReceivePlayer", matchId, playerDto);

            if (matchStatus.IsStarted)
            {
                await Clients.Group($"{matchStatus.GameType}").SendAsync("StartMatch", matchId);
            }
        }

        [Authorize]
        public async Task MakeMove(Guid matchId, string jsonData)
        {
            var playerId = Guid.Parse(Context.User?.FindFirst("Id")?.Value);
            var moveResult = await _matchService.UpdateBoardAsync(matchId, playerId, jsonData);

            if (!moveResult.IsValid) return;

            _matchTimer.PauseTurnTimer(moveResult.GameType, playerId);

            var nextPlayerId = await _matchService.SwtichTurnAsync(matchId, playerId);
            var duration = _matchTimer.StartTurnTimer(moveResult.GameType, matchId, nextPlayerId);

            await Clients.Group($"{moveResult.GameType}").SendAsync("ReceiveMove", moveResult.MoveData, nextPlayerId, duration);
        }

        [Authorize]
        public async Task LeaveMatch(Guid matchId)
        {
            var playerId = Guid.Parse(Context.User.FindFirst("Id").Value);

            await _matchService.RemovePlayerAsync(matchId, playerId);

            await Clients.Group($"{matchId}").SendAsync("RemovePlayer", matchId, playerId);
        }
    }
}
