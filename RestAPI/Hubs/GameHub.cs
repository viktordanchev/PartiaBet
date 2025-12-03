using Games.Dtos;
using Games.Dtos.Request;
using Games.Services;
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

        public async Task<Guid> CreateMatch(CreateMatchRequestDto matchData, AddPlayerRequestDto playerData)
        {
            var newMatch = _matchManagerService.AddMatch(matchData);
            var personData = _matchManagerService.AddPersonToMatch(newMatch.Id, playerData);

            newMatch.Players.Add(personData);

            await Clients.All.SendAsync("ReceiveMatch", newMatch);

            return newMatch.Id;
        }

        public async Task JoinMatch(Guid matchId, AddPlayerRequestDto playerData)
        {
            var playerResponse = _matchManagerService.AddPersonToMatch(matchId, playerData);

            await Clients.All.SendAsync("ReceiveNewPlayer", playerResponse);
        }

        public async Task MakeMove(Guid matchId, string playerId, string jsonData)
        {
            var gameType = _matchManagerService.GetGame(matchId);
            BaseMakeMoveDto moveData;

            try
            {
                moveData = GameFactory.GetMakeMoveDto(gameType, jsonData);
                _matchManagerService.UpdateMatchBoard(matchId, moveData);
            }
            catch
            {
                throw new HubException();
            }

            await Clients.All.SendAsync("ReceiveMove", moveData);
        }
    }
}
