using Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using RestAPI.Dtos.Chat;
using RestAPI.Dtos.Match;
using RestAPI.Services.Interfaces;

namespace RestAPI.Hubs
{
    public class AppHub : Hub
    {
        private readonly IMatchRealtimeService _matchRealtimeService;
        private readonly IPresenceRealtimeService _presenceRealtimeService;
        private readonly IChatRealtimeService _chatRealtimeService;

        public AppHub(IMatchRealtimeService matchRealtimeService,
            IPresenceRealtimeService presenceRealtimeService,
            IChatRealtimeService chatRealtimeService)
        {
            _matchRealtimeService = matchRealtimeService;
            _presenceRealtimeService = presenceRealtimeService;
            _chatRealtimeService = chatRealtimeService;
        }

        public override async Task OnConnectedAsync()
        {
            var user = Context.User?.FindFirst("Id")?.Value;

            if (user != null)
            {
                var userId = Guid.Parse(user);

                await Task.WhenAll(
                    _matchRealtimeService.OnConnectedAsync(userId, Context.ConnectionId),
                    _presenceRealtimeService.OnConnectedAsync(userId, Context.ConnectionId));
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Guid.Parse(Context.User?.FindFirst("Id")?.Value);

            await Task.WhenAll(
                _matchRealtimeService.OnDisconnectedAsync(userId, Context.ConnectionId),
                _presenceRealtimeService.OnDisconnectedAsync(userId, Context.ConnectionId));

            await base.OnDisconnectedAsync(exception);
        }

        //Match methods

        public async Task JoinGameGroup(GameType gameType) =>
            await _matchRealtimeService.JoinGameGroupAsync(Context.ConnectionId, gameType);

        public async Task JoinMatchGroup(Guid matchId) => 
            await _matchRealtimeService.JoinMatchGroupAsync(Context.ConnectionId, matchId);

        [Authorize]
        public async Task<Guid> CreateMatch(AddMatchDto data)
        {
            var userId = Guid.Parse(Context.User?.FindFirst("Id")?.Value);

            var matchId = await _matchRealtimeService.CreateMatchAsync(userId, Context.ConnectionId, data);

            return matchId;
        }

        [Authorize]
        public async Task JoinMatch(Guid matchId)
        {
            var userId = Guid.Parse(Context.User?.FindFirst("Id")?.Value);

            await _matchRealtimeService.JoinMatchAsync(userId, Context.ConnectionId, matchId);
        }

        [Authorize]
        public async Task MakeMove(Guid matchId, string jsonData)
        {
            var userId = Guid.Parse(Context.User?.FindFirst("Id")?.Value);
            
            await _matchRealtimeService.MakeMoveAsync(userId, matchId, jsonData);
        }

        [Authorize]
        public async Task LeaveMatchQueue()
        {
            var userId = Guid.Parse(Context.User.FindFirst("Id").Value);

            await _matchRealtimeService.LeaveMatchQueueAsync(userId);
        }

        [Authorize]
        public async Task RejoinMatch()
        {
            var userId = Guid.Parse(Context.User.FindFirst("Id").Value);

            await _matchRealtimeService.RejoinMatchAsync(userId, Context.ConnectionId);
        }

        //Chat methods

        [Authorize]
        public async Task SendMessage(AddMessageDto data)
        {
            var userId = Guid.Parse(Context.User?.FindFirst("Id")?.Value);

            await _chatRealtimeService.SendMessage(userId, data);
        }

        [Authorize]
        public async Task SenderTyping(Guid receiverId)
        {
            await _chatRealtimeService.SenderTyping(receiverId);
        }
    }
}
