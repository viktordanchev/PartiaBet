using Common.Constants;
using Common.Exceptions;
using Core.DTOs.Requests.Account;
using Core.DTOs.Shared;
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
                    PasswordHash = data.Password,
                    RegisteredAt = data.RegisteredAt
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

        public async Task<UserDTO> GetByEmailAsync(string email)
        {
            return await _database.Users
                .AsNoTracking()
                .Where(u => u.Email == email)
                .Select(u => new UserDTO()
                {
                    Email = email,
                    Username = u.Username,
                    PasswordHash = u.PasswordHash,
                    RegisteredAt = u.RegisteredAt,
                    ImageUrl = u.ImageUrl,
                    Balance = u.Balance,
                    Penalty = u.Penalty
                })
                .FirstAsync();
        }
    }
}
