using Infrastructure.Database.Configs;
using Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database
{
    public class PartiaBetDbContext : DbContext
    {
        public PartiaBetDbContext(DbContextOptions<PartiaBetDbContext> options) 
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UsersRoles { get; set; }
        public DbSet<UserRoleType> UserRoleTypes { get; set; }
        public DbSet<Bet> Bets { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new FriendshipConfig());
            builder.ApplyConfiguration(new UserConfig());

            base.OnModelCreating(builder);
        }
    }
}
