using RestAPI.Extensions;
using RestAPI.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContextExtension(builder.Configuration, builder.Environment);
builder.Services.AddJWTAuthentication(builder.Configuration);
builder.Services.AddCorsExtension(builder.Configuration);
builder.Services.AddControllersExtension();
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddMemoryCache();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

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
app.MapHub<GameHub>("/game");

app.Run();
