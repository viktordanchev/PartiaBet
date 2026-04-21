using Core.Interfaces.Infrastructure;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.SignalR;
using RestAPI.Hubs;
using RestAPI.Services.Interfaces;
using static Common.Constants.Constants;

namespace RestAPI.Services
{
    public class PresenceRealtimeService : IPresenceRealtimeService
    {
        private readonly IOnlineUsersCache _cache;
        private readonly IFriendshipService _friendshipService;
        private readonly IAccountService _accountService;
        private readonly IUserConnectionTracker _userConnectionTracker;
        private readonly IHubContext<AppHub> _hub;

        public PresenceRealtimeService(
            IOnlineUsersCache cache,
            IFriendshipService friendshipService,
            IAccountService accountService,
            IUserConnectionTracker userConnectionTracker,
            IHubContext<AppHub> hub)
        {
            _cache = cache;
            _friendshipService = friendshipService;
            _accountService = accountService;
            _userConnectionTracker = userConnectionTracker;
            _hub = hub;
        }

        public async Task OnConnectedAsync(Guid userId, string connectionId)
        {
            _userConnectionTracker.AddConnection(userId, "presence", connectionId);

            if (_userConnectionTracker.GetConnectionCount(userId, "presence") == 1)
            {
                await _cache.SetUserOnlineAsync(userId);
                await SendFriendStatusUpdate(userId, true);
            }
        }

        public async Task OnDisconnectedAsync(Guid userId, string connectionId)
        {
            await Task.Delay(TimeSpan.FromSeconds(GracePeriodSeconds));

            _userConnectionTracker.RemoveConnection(userId, "presence", connectionId);

            if (_userConnectionTracker.HasConnections(userId, "presence"))
            {
                return;
            }

            await _cache.SetUserOfflineAsync(userId);

            await SendFriendStatusUpdate(userId, false);
        }

        private async Task SendFriendStatusUpdate(Guid userId, bool isOnline)
        {
            var friends = await _friendshipService.GetUserFriendsAsync(userId);
            var userData = await _accountService.GetUserDataAsync(userId);

            var tasks = friends.Select(friend =>
                _hub.Clients.User(friend.ToString())
                    .SendAsync("FriendStatusChange", userData, isOnline));

            await Task.WhenAll(tasks);
        }
    }
}
