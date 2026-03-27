using Core.Interfaces.Infrastructure;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace RestAPI.Hubs
{
    [Authorize]
    public class PresenceHub : Hub
    {
        private readonly IOnlineUsersCache _cache;
        private readonly IFriendshipService _friendshipService;
        private readonly IAccountService _accountService;
        private readonly IUserConnectionTracker _userConnectionTracker;

        public PresenceHub(IOnlineUsersCache cache,
            IFriendshipService friendshipService,
            IAccountService accountService,
            IUserConnectionTracker userConnectionTracker)
        {
            _cache = cache;
            _friendshipService = friendshipService;
            _accountService = accountService;
            _userConnectionTracker = userConnectionTracker;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Guid.Parse(Context.User?.FindFirst("Id")?.Value);
            var userEmail = Context.User?.FindFirst("Email")?.Value;

            _userConnectionTracker.AddConnection(userId);

            if (_userConnectionTracker.GetConnectionCount(userId) == 1)
            {
                await _cache.SetUserOnlineAsync(userId);
                await SendFriendStatusUpdate(userId, true);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Guid.Parse(Context.User?.FindFirst("Id")?.Value);
            var userEmail = Context.User?.FindFirst("Email")?.Value;

            await Task.Delay(5000);

            _userConnectionTracker.RemoveConnection(userId);

            if (_userConnectionTracker.HasConnections(userId))
            {
                await base.OnDisconnectedAsync(exception);
                return;
            }

            await _cache.SetUserOfflineAsync(userId);

            await SendFriendStatusUpdate(userId, false);

            await base.OnDisconnectedAsync(exception);
        }

        private async Task SendFriendStatusUpdate(Guid userId, bool isOnline)
        {
            var friends = await _friendshipService.GetUserFriendsAsync(userId);
            var userData = await _accountService.GetUserDataAsync(Context.User?.FindFirst("Email")?.Value);

            var tasks = friends.Select(friend =>
                 Clients.User(friend.ToString())
                     .SendAsync("FriendStatusChange", userData, isOnline));

            await Task.WhenAll(tasks);
        }
    }
}
