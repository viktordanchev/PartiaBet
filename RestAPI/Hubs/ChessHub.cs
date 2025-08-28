using Core.DTOs;
using Core.Games.Chess;
using Core.Interfaces.Games;
using Infrastructure.Database.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace RestAPI.Hubs
{
    public class ChessHub : Hub  
    {
        private readonly IGameManagerService _gameManagerService;
        private readonly ConcurrentDictionary<string, string> connections;

        public ChessHub(IGameManagerService gameManagerService)
        {
            _gameManagerService = gameManagerService;
            connections = new ConcurrentDictionary<string, string>();
        }

        public async Task CreateMatch(string game, PlayerDto player)
        {
            var gameId = _gameManagerService.CreateGame(gameType, creator);

            await Groups.AddToGroupAsync(Context.ConnectionId, gameId);

            // Изпращаме събитие към всички клиенти, че има нова игра
            await Clients.All.SendAsync("NewGameCreated", new { gameId, gameType, creator });
        }

        public async Task JoinMatch(string game, PlayerDto player)
        {
            _chessService.JoinGame(gameId, player);

            await Groups.AddToGroupAsync(Context.ConnectionId, gameId);

            // изпращаме целия обект към всички играчи в групата
            await Clients.Group(gameId).SendAsync("PlayerJoined", player);
        }

        public override Task OnConnectedAsync()
        {
            connections[Context.UserIdentifier] = Context.ConnectionId;

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            string userId = Context.UserIdentifier ?? Context.ConnectionId;

            connections.TryRemove(userId, out _);

            return base.OnDisconnectedAsync(exception);
        }
    }
}
