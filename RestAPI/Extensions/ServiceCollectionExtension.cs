using Core.Interfaces.ExternalServices;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Services;
using Infrastructure.Database;
using Infrastructure.Database.Repositories;
using Infrastructure.Services;
using Infrastructure.Services.Configs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestAPI.Filters;
using System.Text;

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

        public static void AddJWTAuthentication(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = config["JwtToken:Issuer"],
                    ValidAudience = config["JwtToken:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtToken:Key"]!)),
                    ClockSkew = TimeSpan.Zero
                };
            });
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<IEmailSenderService, EmailSenderService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAccountTokenService, AccountTokenService>();
        }

        public static void AddCorsExtension(this IServiceCollection services, IConfiguration config)
        {
            var allowedOrigin = config["AllowedOrigin"]!;

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.WithOrigins(allowedOrigin)
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials();
                });
            });
        }

        public static void AddControllersExtension(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add<ExceptionFilter>();
                options.Filters.Add<ModelStateValidationFilter>();
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
        }

        public static void AddConfigs(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<EmailSenderConfig>(config.GetSection("EmailSettings"));
        }
    }
}
