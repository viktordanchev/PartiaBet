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
            var exists = await _context.Friendship
                .AnyAsync(f => (f.FirstUserId == senderId && f.SecondUserId == receiverId) ||
                (f.FirstUserId == receiverId && f.SecondUserId == senderId));

            if (exists)
                return;

            var newFirendship = new Friendship()
            {
                FirstUserId = senderId,
                SecondUserId = receiverId,
                CreatedAt = DateTime.UtcNow,
                Status = FriendshipStatus.Pending
            };

            _context.Add(newFirendship);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveFriendship(Guid userId, Guid friendId)
        {
            var friendship = await _context.Friendship
                .Where(f =>
                    (f.FirstUserId == userId && f.SecondUserId == friendId) ||
                    (f.SecondUserId == userId && f.FirstUserId == friendId))
                .FirstOrDefaultAsync();

            if (friendship == null)
                return;

            _context.Remove(friendship);
            await _context.SaveChangesAsync();
        }

        public async Task ChangeStatusAsync(Guid senderId, Guid receiverId, FriendshipStatus status)
        {
            var friendship = await _context.Friendship
                .Where(f => f.FirstUserId == senderId && f.SecondUserId == receiverId)
                .FirstOrDefaultAsync();

            if (friendship == null)
                return;

            friendship.Status = status;

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<FriendModel>> GetFriendsAsync(Guid userId)
        {
            var friends = await _context.Friendship
                .Where(f => f.FirstUserId == userId && f.Status == FriendshipStatus.Accepted)
                .Select(f => new FriendModel()
                {
                    Id = f.SecondUserId,
                    Username = f.SecondUser.Username,
                    ProfileImageUrl = f.SecondUser.ImageUrl
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

        public async Task<PlayerDataModel?> GetPlayerDataAsync(Guid requesterId, Guid playerId)
        {
            var friendshipStatus = await _context.Friendship
                .Where(f =>
                    (f.FirstUserId == requesterId && f.SecondUserId == playerId) ||
                    (f.SecondUserId == requesterId && f.FirstUserId == playerId))
                .Select(f => (FriendshipStatus?)f.Status)
                .FirstOrDefaultAsync() ?? FriendshipStatus.None;

            var player = await _context.Users
                .Where(u => u.Id == playerId)
                .Select(u => new PlayerDataModel
                {
                    Id = u.Id,
                    ProfileImageUrl = u.ImageUrl,
                    Username = u.Username,
                    FriendshipStatus = friendshipStatus,
                    GamesStats = u.GameRatings.Select(f => new GameStatsModel
                    {
                        GameType = f.GameType,
                        Rating = f.Rating,
                        WinCount = u.MatchHistory
                            .Count(um => um.Match.GameType == f.GameType
                                         && um.MatchResult == MatchResult.Win),
                        LossCount = u.MatchHistory
                            .Count(um => um.Match.GameType == f.GameType
                                         && um.MatchResult == MatchResult.Lose)
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            return player;
        }
    }
}
