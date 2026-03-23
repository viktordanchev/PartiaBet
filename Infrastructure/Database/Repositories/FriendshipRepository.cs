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

        public async Task ChangeStatusAsync(Guid senderId, Guid receiverId, FriendshipStatus status)
        {
            var friendship = await _context.Friendship
                .Where(f => f.UserId == senderId && f.FriendId == receiverId)
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

        public async Task<PlayerDataModel?> GetPlayerDataAsync(Guid requesterId, Guid playerId)
        {
            var player = await _context.Users
                .Where(u => u.Id == playerId)
                .Select(u => new PlayerDataModel
                {
                    Id = u.Id,
                    ProfileImageUrl = u.ImageUrl,
                    Username = u.Username,
                    IsFriend = u.Friendships
                        .Any(f => (f.UserId == requesterId || f.FriendId == requesterId)
                                  && f.Status == FriendshipStatus.Accepted),
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
