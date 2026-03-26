using Core.Models.Friendship;

namespace Core.Interfaces.Services
{
    public interface IFriendshipService
    {
        Task SendFriendRequestAsync(Guid senderId, Guid receiverId);
        Task RemoveFriendAsync(Guid firstUserId, Guid secondUserId);
        Task AcceptFriendRequestAsync(Guid senderId, Guid receiverId);
        Task<IEnumerable<FriendModel>> GetFriendshipsAsync(Guid userId);
        Task<IEnumerable<Guid>> GetUserFriendsAsync(Guid userId);
        Task<IEnumerable<FriendModel>> GetAllUsersAsync(string searchQuery);
        Task<PlayerDataModel?> GetPlayerProfileAsync(Guid requesterId, Guid playerId);
    }
}
