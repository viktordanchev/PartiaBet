using Core.Interfaces.Infrastructure;
using Core.Interfaces.Services;
using Core.Enums;
using Core.Models.Friendship;

namespace Core.Services
{
    public class FriendshipService : IFriendshipService
    {
        private IFriendshipRepository _friendshipRepo;
        private IOnlineUsersCache _onlineUsersCache;

        public FriendshipService(
            IFriendshipRepository friendshipRepo,
            IOnlineUsersCache onlineUsersCache)
        {
            _friendshipRepo = friendshipRepo;
            _onlineUsersCache = onlineUsersCache;
        }

        public async Task SendFriendRequestAsync(Guid userId, Guid friendId)
        {
            await _friendshipRepo.AddFriendship(userId, friendId);
        }

        public async Task AcceptFriendRequestAsync(Guid userId, Guid friendId) 
        {
            await _friendshipRepo.ChangeStatusAsync(userId, friendId, FriendshipStatus.Accepted);
        }

        public async Task RemoveFriendAsync(Guid userId, Guid friendId) 
        {
            await _friendshipRepo.RemoveFriendship(userId, friendId);
        }

        public async Task<IEnumerable<FriendModel>> GetFriendsAsync(Guid userId) 
        {
            var friends = await _friendshipRepo.GetFriendsAsync(userId);
            var friendIds = friends.Select(f => f.Id);

            var onlineUserIds = await _onlineUsersCache.GetOnlineUsersAsync(friendIds);

            foreach (var friend in friends)
            {
                friend.IsOnline = onlineUserIds.Contains(friend.Id);
            }

            return friends;
        }

        public async Task<IEnumerable<FriendModel>> GetAllUsersAsync(string searchQuery)
        {
            var users = await _friendshipRepo.GetAllUsersAsync(searchQuery.ToLower());

            return users;
        }
    }
}
