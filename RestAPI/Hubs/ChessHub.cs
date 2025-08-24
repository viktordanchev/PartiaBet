using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace RestAPI.Hubs
{
    public class ChessHub : Hub  
    {
        private static readonly ConcurrentDictionary<string, string> _connections = new ConcurrentDictionary<string, string>();


    }
}
