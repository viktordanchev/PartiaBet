using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace RestAPI.Hubs
{
    public class ChessHub : Hub  
    {
        private static readonly ConcurrentDictionary<string, string> _connections = new ConcurrentDictionary<string, string>();

        public async Task JoinGame(string playerId)
        {
            //if (_connections.TryGetValue(request.ReceiverId, out var connectionId))
            //{
            //    await Clients.Client(connectionId).SendAsync("ReceiveMessage", request.SenderId, request.Message, request.DateAndTime, request.ImageUrls);
            //}
        }
    }
}
