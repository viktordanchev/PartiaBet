using Games.Chess.Models;
using Games.Dtos.Request;
using Interfaces.Games;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

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
            var personData = _matchManagerService.AddPersonToMatch(newMatch.Id, playerData);

            newMatch.Players.Add(personData);

            await Clients.All.SendAsync("ReceiveMatch", newMatch);

            return newMatch.Id;
        }

        public async Task JoinMatch(Guid matchId, AddPlayerRequest playerData)
        {
            var playerResponse = _matchManagerService.AddPersonToMatch(matchId, playerData);

            await Clients.All.SendAsync("UpdatePlayers", playerResponse);
        }

        public async Task MakeMove(Guid matchId, string playerId, string jsonData)
        {
            try
            {
                var move = JsonSerializer.Deserialize<NewMoveDto>(
                    jsonData,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                await Clients.All.SendAsync("ReceiveMove", move);
            }
            catch
            {
                throw new HubException();
            }
        }
    }
}
