using Microsoft.AspNetCore.SignalR;

namespace RestAPI.Services
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirst("Id")?.Value;
        }
    }
}
