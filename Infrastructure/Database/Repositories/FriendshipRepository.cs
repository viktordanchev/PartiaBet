using Core.Interfaces.Infrastructure;
using Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repositories
{
    public class FriendshipRepository : IFriendshipRepository 
    {
        private readonly PartiaBetDbContext _context;

        public FriendshipRepository(PartiaBetDbContext context)
        {
            _context = context;
        }

        public async Task MakeNewFriendship(Guid senderId, Guid receiverId)
        {
            var newFirendship = new Friendship()
            {
                UserId = senderId,
                FriendId = receiverId,
                CreatedAt = DateTime.UtcNow,
            };

            _context.Add(newFirendship);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveFriendship(Guid userId, Guid friendId)
        {
            var friendship = await _context.Friendship
                .Where(f => f.UserId == userId && f.FriendId == friendId)
                .FirstOrDefaultAsync();

            if (friendship == null)
                return;

            _context.Remove(friendship);
            await _context.SaveChangesAsync();
        }
    }
}
