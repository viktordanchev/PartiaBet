using Core.Interfaces.Infrastructure;
using Core.Interfaces.Services;
using Core.Models.Chat;

namespace Core.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;

        public ChatService(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public async Task<MessageDataModel> AddMessageAsync(Guid senderId, Guid receiverId, string message)
        {
            return await _chatRepository.AddMessageAsync(senderId, receiverId, message);
        }

        public async Task<IEnumerable<MessageDataModel>> GetChatHistoryAsync(Guid senderId, Guid receiverId)
        {
            return await _chatRepository.GetAllMessagesAsync(senderId, receiverId);
        }
    }
}
