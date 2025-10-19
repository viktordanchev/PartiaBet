using Games.Dtos.Request;
using Interfaces.Games;
using Microsoft.AspNetCore.SignalR;

namespace RestAPI.Hubs
{
    public class GameHub : Hub  
    {
        private readonly IMatchManagerService _matchManagerService;

        public GameHub(IMatchManagerService gameManagerService)
        {
            _matchManagerService = gameManagerService;
        }

        public async Task<Guid> CreateMatch(CreateMatchRequest matchData, AddPlayerRequest playerData)
        {
            var newMatch = _matchManagerService.AddMatch(matchData);
            var personData = _matchManagerService.AddPersonToMatch(matchData.GameId, newMatch.Id, playerData);

            newMatch.Players.Add(personData);

            await Clients.All.SendAsync("ReceiveMatch", newMatch);

            return newMatch.Id;
        }

        //public async Task JoinMatch(GameType gameType, Guid matchId, PlayerDto player)
        //{
        //    var match = _matchManagerService.AddPersonToMatch(gameType, matchId, player);
        //
        //    await Clients.All.SendAsync("UpdatePlayerCount", gameType);
        //    await Clients.All.SendAsync("UpdatePlayers", match);
        //}
    }
}
