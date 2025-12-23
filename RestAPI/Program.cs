using RestAPI.Extensions;
using RestAPI.Hubs;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddAutoMapper();
builder.Services.AddDbContextExtension(builder.Configuration, builder.Environment);
builder.Services.AddJWTAuthentication(builder.Configuration);
builder.Services.AddCorsExtension(builder.Configuration);
builder.Services.AddConfigs(builder.Configuration);
builder.Services.AddControllersExtension();
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR()
    .AddJsonProtocol(options =>
    {
        options.PayloadSerializerOptions.Converters.Add(
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
        );
    });
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "PartiaBet";
});

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");
app.MapHub<MatchHub>("/match");

app.Run();
