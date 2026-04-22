using Core.Models.Chat;

namespace Core.Interfaces.Infrastructure
{
    public interface IChatRepository
    {
        Task<Guid> AddMessageAsync(Guid senderId, Guid receiverId, string message);
        Task<IEnumerable<MessageDataModel>> GetAllMessagesAsync(Guid senderId, Guid receiverId);
    }
}
