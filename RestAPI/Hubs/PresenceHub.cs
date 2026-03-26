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

        public PresenceHub(IOnlineUsersCache cache,
            IFriendshipService friendshipService)
        {
            _cache = cache;
            _friendshipService = friendshipService;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Guid.Parse(Context.User?.FindFirst("Id")?.Value);
            await _cache.SetUserOnlineAsync(userId);

            var freinds = await _friendshipService.GetUserFriendsAsync(userId);

            foreach (var friend in freinds)
            {
                await Clients.User(friend.ToString()).SendAsync("FriendGoesOnline", userId);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Guid.Parse(Context.User?.FindFirst("Id")?.Value);
            await _cache.SetUserOfflineAsync(userId);

            var freinds = await _friendshipService.GetUserFriendsAsync(userId);

            foreach (var friend in freinds)
            {
                await Clients.User(friend.ToString()).SendAsync("FriendGoesOffline", userId);
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task Heartbeat()
        {
            var userId = Guid.Parse(Context.User?.FindFirst("Id")?.Value);
            await _cache.SetUserOnlineAsync(userId);
        }
    }
}
