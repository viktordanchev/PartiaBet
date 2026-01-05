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
        private readonly IMapper _mapper;

        public MatchHub(IMatchService matchService, IMapper mapper)
        {
            _matchService = matchService;
            _mapper = mapper;
        }

        public async Task JoinGame(GameType gameType)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"{gameType}");
        }

        [Authorize]
        public async Task<Guid> CreateMatch(AddMatchDto matchData, AddPlayerDto playerData)
        {
            var matchModel = _mapper.Map<AddMatchModel>(matchData);

            var match = await _matchService.CreateMatchAsync(matchModel);
            await _matchService.AddPlayerAsync(match.Id, playerData.Id);

            await Groups.AddToGroupAsync(Context.ConnectionId, $"{match.Id}");
            await Clients.Group($"{matchData.GameType}").SendAsync("ReceiveMatch", match);

            return match.Id;
        }

        [Authorize]
        public async Task JoinMatch(Guid matchId, AddPlayerDto playerData)
        {
            var playerId = Guid.Parse(Context.User?.FindFirst("Id")?.Value);
            var playerResponse = await _matchService.AddPlayerAsync(matchId, playerId);

            await Groups.AddToGroupAsync(Context.ConnectionId, $"{matchId}");
            await Clients.Group($"{matchId}").SendAsync("ReceiveNewPlayer", playerResponse);
        }

        [Authorize]
        public async Task MakeMove(Guid matchId, string jsonData)
        {
            var playerId = Guid.Parse(Context.User?.FindFirst("Id")?.Value);

            var (isValidMove, moveData) = await _matchService.TryMakeMoveAsync(matchId, playerId, jsonData);

            if (isValidMove)
            {
                await Clients.Group($"{matchId}").SendAsync("ReceiveMove", moveData);
            }
        }

        [Authorize]
        public async Task LeaveMatch(Guid matchId)
        {
            var playerId = Guid.Parse(Context.User.FindFirst("Id").Value);
            var isRemoved = await _matchService.RemovePlayerAsync(matchId, playerId);

            if (isRemoved)
            {
                await Clients.Group($"{matchId}").SendAsync("RemovePlayer", playerId);
            }
        }
    }
}
