using Core.Models.Chat;

namespace Core.Interfaces.Services
{
    public interface IChatService
    {
        Task<Guid> AddMessageAsync(Guid senderId, Guid receiverId, string message);
        Task<IEnumerable<MessageDataModel>> GetChatHistoryAsync(Guid senderId, Guid receiverId);
    }
}
