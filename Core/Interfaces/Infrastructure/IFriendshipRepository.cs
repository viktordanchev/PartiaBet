namespace Core.Interfaces.Infrastructure
{
    public interface IFriendshipRepository
    {
        Task RemoveFriendship(Guid userId, Guid friendId);
        Task MakeNewFriendship(Guid senderId, Guid receiverId);
    }
}
