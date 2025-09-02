using Common.Constants;
using Common.Exceptions;
using Core.DTOs;
using Core.DTOs.Requests.Account;
using Core.Interfaces.Repositories;
using Infrastructure.Database.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Infrastructure.Database.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly PartiaBetDbContext _database;

        public UserRepository(PartiaBetDbContext database)
        {
            _database = database;
        }

        public async Task AddUserAsync(RegisterUserRequest data)
        {
            try
            {
                await _database.Users.AddAsync(
                new User
                {
                    Email = data.Email,
                    Username = data.Email,
                    PasswordHash = data.Password
                });
                await _database.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx)
            {
                var message = pgEx.ConstraintName == "UX_User_Username"
                    ? ErrorMessages.UsedUsername : ErrorMessages.UsedEmail;

                throw new ApiException(message, StatusCodes.Status409Conflict);
            }
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

        public async Task<UserClaimsDto> GetUserClaimsByEmailAsync(string email)
        {
            return await _database.Users
                .AsNoTracking()
                .Where(u => u.Email == email)
                .Select(u => new UserClaimsDto
                {
                    Id = u.Id.ToString(),
                    Email = u.Email,
                    Username = u.Username,
                    Roles = u.Roles.Select(r => r.RoleType.Name).ToList()
                })
                .FirstAsync();
        }
    }
}
