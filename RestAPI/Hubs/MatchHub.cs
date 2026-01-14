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

        public async Task JoinGame(GameType gameType)
        {
            if (_gameProvider.IsValidGameType(gameType))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"{gameType}");
            }
        }

        [Authorize]
        public async Task<Guid> CreateMatch(AddMatchDto data)
        {
            var playerId = Guid.Parse(Context.User?.FindFirst("Id")?.Value);

            var match = await _matchService.CreateMatchAsync(data.GameType, data.BetAmount);
            var player = await _matchService.AddPlayerAsync(match.Id, playerId);

            var matchDto = _mapper.Map<MatchDto>(match);
            var playerDto = _mapper.Map<PlayerDto>(player);
            matchDto.Players.Add(playerDto);

            await Groups.AddToGroupAsync(Context.ConnectionId, $"{match.Id}");
            await Clients.Group($"{match.GameType}").SendAsync("ReceiveMatch", matchDto);

            return match.Id;
        }

        [Authorize]
        public async Task JoinMatch(Guid matchId)
        {
            var playerId = Guid.Parse(Context.User?.FindFirst("Id")?.Value);
            
            var player = await _matchService.AddPlayerAsync(matchId, playerId);

            var playerDto = _mapper.Map<PlayerDto>(player);

            await Groups.AddToGroupAsync(Context.ConnectionId, $"{matchId}");
            await Clients.Group($"{matchId}").SendAsync("ReceiveNewPlayer", playerDto);
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

            await Clients.Group($"{matchId}").SendAsync("ReceiveMove", moveResult.MoveData, nextPlayerId, duration);
        }

        //[Authorize]
        //public async Task LeaveMatch(Guid matchId)
        //{
        //    var playerId = Guid.Parse(Context.User.FindFirst("Id").Value);
        //    var match = await _matchService.GetMatchInternalAsync(matchId);
        //    
        //    if (match.MatchStatus == MatchStatus.Ongoing)
        //    {
        //        _matchTimer.StartLeaverTimer(match.GameType, match.Id, playerId);
        //        await _matchService.UpdatePlayerStatusAsync(playerId, PlayerStatus.Disconnected);
        //    }
        //    else
        //    {
        //        await _matchService.RemovePlayerAsync(match, playerId);
        //    }
        //}
    }
}
