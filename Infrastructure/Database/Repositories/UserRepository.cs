using Core.Interfaces.Infrastructure;
using Core.Models.User;
using Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Infrastructure.Database.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly PartiaBetDbContext _database;

        public UserRepository(PartiaBetDbContext database)
        {
            _database = database;
        }

        public async Task AddUserAsync(RegisterUserModel data)
        {
            var user = new User
            {
                Email = data.Email,
                Username = data.Username,
                RegisteredAt = DateTime.Parse(data.DateAndTime, new CultureInfo("bg-BG")),
                PasswordHash = data.Password
            };

            await _database.Users.AddAsync(user);
            await _database.SaveChangesAsync();
        }

        public async Task<(bool emailExists, bool usernameExists)> IsUserDataUniqueAsync(string email, string username)
        {
            var existingUsers = await _database.Users
                .Where(u => u.Email == email || u.Username == username)
                .ToListAsync();

            var emailExists = existingUsers.Any(u => u.Email == email);
            var usernameExists = existingUsers.Any(u => u.Username == username);

            return (emailExists, usernameExists);
        }

        public async Task<bool> IsUserExistAsync(string email)
        {
            return await _database.Users.AnyAsync(u => u.Email == email);
        }

        public async Task UpdatePasswordAsync(string email, string passwordHash)
        {
            var user = await _database.Users.FirstAsync(u => u.Email == email);

            user.PasswordHash = passwordHash;

            await _database.SaveChangesAsync();
        }

        public async Task<string?> GetUserPasswordHashAsync(string email)
        {
            return await _database.Users
                .AsNoTracking()
                .Where(u => u.Email == email)
                .Select(u => u.PasswordHash)
                .FirstOrDefaultAsync();
        }

        public async Task<UserClaimsModel> GetUserClaimsByEmailAsync(string email)
        {
            return await _database.Users
                .AsNoTracking()
                .Where(u => u.Email == email)
                .Select(u => new UserClaimsModel
                {
                    Id = u.Id.ToString(),
                    Email = u.Email,
                    Username = u.Username,
                    ImageUrl = u.ImageUrl ?? string.Empty,
                    Roles = u.Roles.Select(r => r.RoleType.Name).ToList()
                })
                .FirstAsync();
        }
    }
}
