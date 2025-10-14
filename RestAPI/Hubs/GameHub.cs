using Core.Enums;
using Games.Dtos;
using Interfaces.Games;
using Microsoft.AspNetCore.SignalR;

namespace RestAPI.Hubs
{
    public class GameHub : Hub  
    {
        private readonly IMatchManagerService _gameManagerService;

        public GameHub(IMatchManagerService gameManagerService)
        {
            _gameManagerService = gameManagerService;
        }

        public async Task CreateMatch(MatchDto match, PlayerDto hostPlayer)
        {
            var matchId = _gameManagerService.AddMatch(match);
            var createdMatch = _gameManagerService.AddPersonToMatch(match.GameType, matchId, hostPlayer);

            await Clients.All.SendAsync("ReceiveMatch", createdMatch);
        }

        public async Task JoinMatch(GameType gameType, Guid matchId, PlayerDto player)
        {
            var match = _gameManagerService.AddPersonToMatch(gameType, matchId, player);

            await Clients.All.SendAsync("UpdatePlayerCount", gameType);
            await Clients.All.SendAsync("UpdatePlayers", match);
        }
    }
}
