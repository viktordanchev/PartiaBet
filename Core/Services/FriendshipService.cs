using Core.Enums;
using Core.Interfaces.Games;
using Core.Interfaces.Infrastructure;
using Core.Interfaces.Services;
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

        public async Task SendFriendRequestAsync(Guid senderId, Guid receiverId)
        {
            await _friendshipRepo.AddFriendship(senderId, receiverId);
        }

        public async Task AcceptFriendRequestAsync(Guid senderId, Guid receiverId) 
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
            if (string.IsNullOrWhiteSpace(searchQuery))
                return new List<FriendModel>();

            var users = await _friendshipRepo.GetAllUsersAsync(searchQuery.ToLower());

            return users;
        }

        public async Task<PlayerDataModel?> GetPlayerProfileAsync(Guid requesterId, Guid playerId) 
        {
            var player = await _friendshipRepo.GetPlayerDataAsync(requesterId, playerId);

            return player;
        }
    }
}
