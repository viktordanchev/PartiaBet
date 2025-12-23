using AutoMapper;
using Core.Interfaces.Games;
using Core.Interfaces.Services;
using Core.Models.Match;
using Games.Factories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using RestAPI.Dtos.Match;

namespace RestAPI.Hubs
{
    public class MatchHub : Hub
    {
        private readonly IMatchService _matchService;
        private readonly IMapper _mapper;
        private readonly IGameFactory _gameFactory;

        public MatchHub(IMatchService matchService, IMapper mapper, IGameFactory gameFactory)
        {
            _matchService = matchService;
            _mapper = mapper;
            _gameFactory = gameFactory;
        }

        public async Task JoinGame(int gameId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"game-{gameId}");
        }

        [Authorize]
        public async Task<Guid> CreateMatch(AddMatchDto matchData, AddPlayerDto playerData)
        {
            var matchModel = _mapper.Map<AddMatchModel>(matchData);

            var match = await _matchService.CreateMatchAsync(matchModel);
            await _matchService.AddPersonToMatch(match.Id, playerData.Id);

            await Clients.Group($"game-{matchData.GameId}").SendAsync("ReceiveMatch", match);

            return match.Id;
        }

        [Authorize]
        public async Task JoinMatch(Guid matchId, AddPlayerDto playerData)
        {
            var playerResponse = await _matchService.AddPersonToMatch(matchId, playerData.Id);

            await Clients.All.SendAsync("ReceiveNewPlayer", playerResponse);
        }

        [Authorize]
        public async Task MakeMove(Guid matchId, string jsonData)
        {
            BaseMoveModel moveData;

            try
            {
                var gameType = await _matchService.GetMatchGameTypeAsync(matchId);
                moveData = _gameFactory.GetMakeMoveDto(gameType, jsonData);
            }
            catch
            {
                throw new HubException();
            }

            var playerId = Context.User?.FindFirst("Id")?.Value;
            var game = await _matchService.GetMatchGameTypeAsync(matchId);

            await _matchService.TryMakeMove(matchId, game, playerId, moveData);

            await Clients.All.SendAsync("ReceiveMove", moveData);
        }
    }
}
