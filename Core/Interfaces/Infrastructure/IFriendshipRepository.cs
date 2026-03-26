using Core.Enums;
using Core.Models.Friendship;

namespace Core.Interfaces.Infrastructure
{
    public interface IFriendshipRepository
    {
        Task RemoveFriendship(Guid firstUserId, Guid secondUserId);
        Task AddFriendship(Guid senderId, Guid receiverId);
        Task ChangeStatusAsync(Guid userId, Guid friendId, FriendshipStatus status);
        Task<IEnumerable<FriendModel>> GetFriendshipsAsync(Guid userId);
        Task<IEnumerable<Guid>> GetUserFriendsAsync(Guid userId);
        Task<IEnumerable<FriendModel>> GetAllUsersAsync(string searchQuery);
        Task<PlayerDataModel?> GetPlayerDataAsync(Guid requesterId, Guid playerId);
    }
}
