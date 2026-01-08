using AutoMapper;
using Core.Enums;
using Core.Interfaces.Services;
using Core.Models.Match;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using RestAPI.Dtos.Match;

namespace RestAPI.Hubs
{
    public class MatchHub : Hub
    {
        private readonly IMatchService _matchService;
        private readonly IMatchTimer _matchTimer;
        private readonly IMapper _mapper;

        public MatchHub(IMatchService matchService, 
            IMatchTimer matchTimer, 
            IMapper mapper)
        {
            _matchService = matchService;
            _matchTimer = matchTimer;
            _mapper = mapper;
        }

        public async Task JoinGame(GameType gameType)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"{gameType}");
        }

        [Authorize]
        public async Task<Guid> CreateMatch(AddMatchDto matchData)
        {
            var playerId = Guid.Parse(Context.User?.FindFirst("Id")?.Value);
            var matchModel = _mapper.Map<AddMatchModel>(matchData);

            var match = await _matchService.CreateMatchAsync(matchModel);
            match.Players.Add(await _matchService.AddPlayerAsync(match, playerId));

            var matchDto = _mapper.Map<MatchDto>(match);

            await Groups.AddToGroupAsync(Context.ConnectionId, $"{match.Id}");
            await Clients.Group($"{matchData.GameType}").SendAsync("ReceiveMatch", matchDto);

            return match.Id;
        }

        [Authorize]
        public async Task JoinMatch(Guid matchId)
        {
            var playerId = Guid.Parse(Context.User?.FindFirst("Id")?.Value);
            var match = await _matchService.GetMatchInternalAsync(matchId);
            var playerResponse = await _matchService.AddPlayerAsync(match, playerId);

            var playerDto = _mapper.Map<PlayerDto>(playerResponse);

            await Groups.AddToGroupAsync(Context.ConnectionId, $"{matchId}");
            await Clients.Group($"{matchId}").SendAsync("ReceiveNewPlayer", playerDto);
        }

        [Authorize]
        public async Task MakeMove(Guid matchId, string jsonData)
        {
            var playerId = Guid.Parse(Context.User?.FindFirst("Id")?.Value);
            var match = await _matchService.GetMatchInternalAsync(matchId);

            _matchTimer.PauseTurnTimer(match.GameType, playerId);

            var moveResult = await _matchService.TryMakeMoveAsync(match, playerId, jsonData);
            
            if (moveResult.IsValid)
            {
                var newPlayerId = await _matchService.SwtichTurnAsync(match, playerId);
                var duration = _matchTimer.StartTurnTimer(match.GameType, newPlayerId, match.Id);

                await Clients.Group($"{matchId}").SendAsync("ReceiveMove", moveResult.MoveData, newPlayerId, duration);
            }
        }

        [Authorize]
        public async Task LeaveMatch(Guid matchId)
        {
            var playerId = Guid.Parse(Context.User.FindFirst("Id").Value);
            var match = await _matchService.GetMatchInternalAsync(matchId);
            
            if (match.MatchStatus == MatchStatus.Ongoing)
            {
                _matchTimer.StartLeaverTimer(match.GameType, match.Id, playerId);
                await _matchService.UpdatePlayerStatusAsync(matchId, playerId, PlayerStatus.Disconnected);
            }
            else
            {
                await _matchService.RemovePlayerAsync(match, playerId);
            }
        }
    }
}
