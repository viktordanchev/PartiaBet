using Core.Interfaces.Repositories;

namespace Infrastructure.Database.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly PartiaBetDbContext _database;

        public UserRepository(PartiaBetDbContext database)
        {
            _database = database;
        }

        public async Task AddUserAsync()
        {
            //await _database.Users.AddAsync(
            //new User
            //{
            //    Email = ,
            //    Username = ,
            //    PasswordHash = ,
            //    ImageUrl = 
            //});
            await _database.SaveChangesAsync();
        }
    }
}
