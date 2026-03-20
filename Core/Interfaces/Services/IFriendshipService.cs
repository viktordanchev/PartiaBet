using Core.Models.Friendship;

namespace Core.Interfaces.Services
{
    public interface IFriendshipService
    {
        Task SendFriendRequestAsync(Guid userId, Guid friendId);
        Task RemoveFriendAsync(Guid userId, Guid friendId);
        Task AcceptFriendRequestAsync(Guid userId, Guid friendId);
        Task<IEnumerable<FriendModel>> GetFriendsAsync(Guid userId);
        Task<IEnumerable<FriendModel>> GetAllUsersAsync(string searchQuery);
        Task<PlayerDataModel?> GetPlayerProfileAsync(Guid requesterId, Guid playerId);
    }
}
