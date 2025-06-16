using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace RestAPI.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddDbContextExtension(this IServiceCollection services, IConfiguration config, IWebHostEnvironment environment)
        {
            var connectionString = environment.IsDevelopment()
                ? config.GetConnectionString("LocalConnection")
                : config.GetConnectionString("RailwayConnection");

            services.AddDbContext<PartiaBetDbContext>(options =>
                options.UseNpgsql(connectionString));
        }
    }
}
