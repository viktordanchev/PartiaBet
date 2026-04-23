using Core.Interfaces.Infrastructure;
using Core.Models.Chat;
using Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly PartiaBetDbContext _context;

        public ChatRepository(PartiaBetDbContext context)
        {
            _context = context;
        }

        public async Task<MessageDataModel> AddMessageAsync(Guid senderId, Guid receiverId, string message)
        {
            var newMessage = new ChatMessage()
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Message = message,
                DateAndTime = DateTime.UtcNow
            };

            _context.ChatMessages.Add(newMessage);
            await _context.SaveChangesAsync();

            return new MessageDataModel()
            {
                Id = newMessage.Id,
                SenderId = senderId,
                DateAndTime = newMessage.DateAndTime,
                Message = message
            };
        }

        public async Task<IEnumerable<MessageDataModel>> GetAllMessagesAsync(Guid senderId, Guid receiverId)
        {
            var messages = await _context.ChatMessages
                .AsNoTracking()
                .Where(cm => cm.SenderId == senderId && cm.ReceiverId == receiverId ||
                    cm.SenderId == receiverId && cm.ReceiverId == senderId)
                .OrderBy(cm => cm.DateAndTime)
                .Select(cm => new MessageDataModel()
                {
                    Id = cm.Id,
                    SenderId = cm.SenderId,
                    DateAndTime = cm.DateAndTime,
                    Message = cm.Message
                })
                .ToListAsync();

            return messages;
        }

    }
}
