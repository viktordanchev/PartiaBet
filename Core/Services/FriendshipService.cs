using Core.Enums;
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
            await _friendshipRepo.ChangeStatusAsync(senderId, receiverId, FriendshipStatus.Accepted);
        }

        public async Task RemoveFriendAsync(Guid firstUserId, Guid secondUserId) 
        {
            await _friendshipRepo.RemoveFriendship(firstUserId, secondUserId);
        }

        public async Task<IEnumerable<FriendModel>> GetFriendshipsAsync(Guid userId) 
        {
            var friends = await _friendshipRepo.GetFriendshipsAsync(userId);
            var friendIds = friends.Select(f => f.Id);

            var onlineUserIds = await _onlineUsersCache.GetOnlineUsersAsync(friendIds);

            foreach (var friend in friends)
            {
                friend.IsOnline = onlineUserIds.Contains(friend.Id);
            }

            return friends;
        }

        public async Task<IEnumerable<Guid>> GetUserFriendsAsync(Guid userId)
        {
            return await _friendshipRepo.GetUserFriendsAsync(userId);
        }

        public async Task<IEnumerable<FriendModel>> GetAllUsersAsync(string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
                return new List<FriendModel>();

            var users = await _friendshipRepo.GetAllUsersAsync(searchQuery.ToLower());
            var usersIds = users.Select(f => f.Id);

            var onlineUserIds = await _onlineUsersCache.GetOnlineUsersAsync(usersIds);

            foreach (var user in users)
            {
                user.IsOnline = onlineUserIds.Contains(user.Id);
            }

            return users;
        }

        public async Task<PlayerDataModel?> GetPlayerProfileAsync(Guid requesterId, Guid playerId) 
        {
            var player = await _friendshipRepo.GetPlayerDataAsync(requesterId, playerId);

            return player;
        }
    }
}
