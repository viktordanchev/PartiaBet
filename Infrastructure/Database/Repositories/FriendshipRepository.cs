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
                RequesterId = senderId,
                CreatedAt = DateTime.UtcNow,
                Status = FriendshipStatus.Pending
            };

            _context.Add(newFirendship);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveFriendship(Guid firstUserId, Guid secondUserId)
        {
            var friendship = await _context.Friendship
                .Where(f =>
                    (f.FirstUserId == firstUserId && f.SecondUserId == secondUserId) ||
                    (f.SecondUserId == firstUserId && f.FirstUserId == secondUserId))
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

        public async Task<IEnumerable<FriendModel>> GetFriendshipsAsync(Guid userId)
        {
            var friends = await _context.Friendship
                .AsNoTracking()
                .Where(f => f.FirstUserId == userId || f.SecondUserId == userId)
                .Select(f => new FriendModel()
                {
                    Id = f.FirstUserId == userId ? f.SecondUserId : f.FirstUserId,
                    Username = f.FirstUserId == userId
                        ? f.SecondUser.Username
                        : f.FirstUser.Username,
                    ProfileImageUrl = f.FirstUserId == userId
                        ? f.SecondUser.ImageUrl
                        : f.FirstUser.ImageUrl,
                    IsFriendRequestPending = f.Status == FriendshipStatus.Pending,
                })
                .OrderBy(f => f.IsFriendRequestPending == true)
                .ToListAsync();

            return friends;
        }

        public async Task<IEnumerable<Guid>> GetUserFriendsAsync(Guid userId)
        {
            var friends = await _context.Friendship
                .AsNoTracking()
                .Where(f => f.FirstUserId == userId || f.SecondUserId == userId)
                .Select(f => f.FirstUserId == userId ? f.SecondUserId : f.FirstUserId)
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
            var friendship = await _context.Friendship
                .Where(f =>
                    (f.FirstUserId == requesterId && f.SecondUserId == playerId) ||
                    (f.SecondUserId == requesterId && f.FirstUserId == playerId))
                .FirstOrDefaultAsync();

            var player = await _context.Users
                .Where(u => u.Id == playerId)
                .Select(u => new PlayerDataModel
                {
                    Id = u.Id,
                    ProfileImageUrl = u.ImageUrl,
                    Username = u.Username,
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

            if (player == null)
                return null;

            player.FriendshipStatus = friendship?.Status ?? FriendshipStatus.None;
            player.FriendshipRequesterId = friendship?.RequesterId;

            return player;
        }
    }
}
