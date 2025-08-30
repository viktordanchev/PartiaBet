using Core.Games.Dtos;
using Core.Interfaces.Games;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace RestAPI.Hubs
{
    public class GameHub : Hub  
    {
        private readonly IGameManagerService _gameManagerService;
        private readonly IMatchService _matchService;
        private readonly ConcurrentDictionary<string, string> connections;

        public GameHub(IGameManagerService gameManagerService, IMatchService matchService)
        {
            _gameManagerService = gameManagerService;
            _matchService = matchService;
            connections = new ConcurrentDictionary<string, string>();
        }

        public async Task CreateMatch(MatchDto match)
        {
            await _matchService.AddMatchAsync(match);
            _gameManagerService.CreateMatch(match);

            await Clients.All.SendAsync("MatchCreated", match);
        }

        public async Task JoinMatch(string gameName, MatchDto match, PlayerDto player)
        {
            _gameManagerService.JoinMatch(match.GameId, match.Id, player);

            await Clients.All.SendAsync("UpdatePlayerCount", gameName, 12);
        }
    }
}
