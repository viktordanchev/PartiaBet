using Core.DTOs.Requests.Account;
using Core.Interfaces.Repositories;
using Infrastructure.Database.Entities;

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
    }
}
