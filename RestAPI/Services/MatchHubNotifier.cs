using Core.Enums;
using Core.Interfaces.Services;
using Core.Results.Match;
using Microsoft.AspNetCore.SignalR;
using RestAPI.Hubs;

namespace RestAPI.Services
{
    public class MatchHubNotifier : IMatchHubNotifier
    {
        private readonly IHubContext<MatchHub> _hubContext;

        public MatchHubNotifier(IHubContext<MatchHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task RemoveMatch(GameType gameType, Guid matchId)
        {
            await _hubContext.Clients.Group($"{gameType}")
                .SendAsync("RemoveMatch", matchId);
        }

        public async Task EndMatch(EndMatchResult result)
        {
            await _hubContext.Clients.Group($"{result.MatchId}")
                .SendAsync("EndMatch", result.Players);
        }
    }
}
