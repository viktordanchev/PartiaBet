using Interfaces.Games;
using Microsoft.AspNetCore.SignalR;

namespace RestAPI.Hubs
{
    public class ChessHub : GameHub
    {
        public ChessHub(IMatchManagerService gameManagerService) : base(gameManagerService)
        {
        }

        public async Task MakeMove(int oldRow, int oldCol, int row, int col)
        {
            await Clients.All.SendAsync("ReceiveMove", oldRow, oldCol, row, col);
        }
    }
}
