using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database
{
    public class PartiaBetDbContext : DbContext
    {
        public PartiaBetDbContext(DbContextOptions<PartiaBetDbContext> options)
            : base(options)
        {
        }
    }
}
