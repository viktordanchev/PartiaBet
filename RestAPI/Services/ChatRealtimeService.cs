using Core.Interfaces.Services;
using Microsoft.AspNetCore.SignalR;
using RestAPI.Dtos.Chat;
using RestAPI.Hubs;
using RestAPI.Services.Interfaces;

namespace RestAPI.Services
{
    public class ChatRealtimeService : IChatRealtimeService
    {
        private readonly IChatService _chatService;
        private readonly IHubContext<AppHub> _hub;

        public ChatRealtimeService(IChatService chatService,
            IHubContext<AppHub> hub)
        {
            _chatService = chatService;
            _hub = hub;
        }

        public async Task SendMessage(Guid senderId, AddMessageDto data)
        {
            var messageModel = await _chatService.AddMessageAsync(senderId, data.ReceiverId, data.Message);

            await _hub.Clients.Users(data.ReceiverId.ToString(), senderId.ToString())
                .SendAsync("ReceiveMessage", messageModel);
        }

        public async Task SenderTyping(Guid receiverId)
        {
            await _hub.Clients.User(receiverId.ToString())
                .SendAsync("SenderTyping");
        }
    }
}
