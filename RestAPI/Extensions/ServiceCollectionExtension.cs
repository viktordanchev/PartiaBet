using Core.Interfaces.ExternalServices;
using Core.Interfaces.Games;
using Core.Interfaces.Infrastructure;
using Core.Interfaces.Services;
using Core.Services;
using Core.Services.Configs;
using Games.Factories;
using Infrastructure.CacheRedis;
using Infrastructure.Database;
using Infrastructure.Database.Repositories;
using Infrastructure.Services;
using Infrastructure.Services.Configs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestAPI.Filters;
using RestAPI.Mapper;
using StackExchange.Redis;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

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

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;

                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/match"))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            });
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMatchRepository, MatchRepository>();
            services.AddScoped<IGamesRepository, GamesRepository>();
            services.AddScoped<ICacheService, CacheService>();
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<IEmailSenderService, EmailSenderService>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAccountTokenService, AccountTokenService>();
            services.AddScoped<IPasswordHasher<object>, PasswordHasher<object>>();
            services.AddScoped<IGamesService, GamesService>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IMatchService, MatchService>();
            services.AddScoped<IGameFactory, GameFactory>();

            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var configuration = "localhost:6379";
                return ConnectionMultiplexer.Connect(configuration);
            });
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
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            });
        }

        public static void AddConfigs(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<EmailSenderConfig>(config.GetSection("EmailSettings"));
            services.Configure<JwtTokenConfig>(config.GetSection("JwtToken"));
        }

        public static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => { },
                typeof(UserProfile),
                typeof(GameProfile));
        }
    }
}
