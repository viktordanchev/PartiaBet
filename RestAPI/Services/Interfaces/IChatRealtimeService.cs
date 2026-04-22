using RestAPI.Dtos.Chat;

namespace RestAPI.Services.Interfaces
{
    public interface IChatRealtimeService
    {
        Task SendMessage(Guid senderId, AddMessageDto data);
        Task SenderTyping(Guid receiverId);

    }
}
