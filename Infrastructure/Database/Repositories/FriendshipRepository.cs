using Core.Interfaces.Infrastructure;
using Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Core.Enums;
using Core.Models.Friendship;

namespace Infrastructure.Database.Repositories
{
    public class FriendshipRepository : IFriendshipRepository
    {
        private readonly PartiaBetDbContext _context;

        public FriendshipRepository(PartiaBetDbContext context)
        {
            _context = context;
        }

        public async Task AddFriendship(Guid senderId, Guid receiverId)
        {
            var newFirendship = new Friendship()
            {
                UserId = senderId,
                FriendId = receiverId,
                CreatedAt = DateTime.UtcNow,
                Status = FriendshipStatus.Pending
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

        public async Task ChangeStatusAsync(Guid userId, Guid friendId, FriendshipStatus status)
        {
            var friendship = await _context.Friendship
                .Where(f => f.UserId == userId && f.FriendId == friendId)
                .FirstOrDefaultAsync();

            if (friendship == null)
                return;

            friendship.Status = status;

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<FriendModel>> GetFriendsAsync(Guid userId)
        {
            var friends = await _context.Friendship
                .Where(f => f.UserId == userId && f.Status == FriendshipStatus.Accepted)
                .Select(f => new FriendModel()
                {
                    Id = f.FriendId,
                    Username = f.Friend.Username,
                    ProfileImageUrl = f.Friend.ImageUrl
                })
                .ToListAsync();

            return friends;
        }

        public async Task<IEnumerable<FriendModel>> GetAllUsersAsync(string searchQuery)
        {
            var users = await _context.Users
                .Where(u => u.Username.ToLower().Contains(searchQuery))
                .Select(u => new FriendModel()
                {
                    Id = u.Id,
                    Username = u.Username,
                    ProfileImageUrl = u.ImageUrl
                })
                .ToListAsync();

            return users;
        }
    }
}
