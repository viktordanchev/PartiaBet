using Core.Enums;
using Games.Dtos.Request.Matches;
using Interfaces.Games;
using Microsoft.AspNetCore.Mvc;

namespace RestAPI.Controllers
{
    [ApiController]
    [Route("api/matches")]
    public class MatchesController : Controller
    {
        private readonly IMatchManagerService _matchManagerService;

        public MatchesController(IMatchManagerService gameManagerService)
        {
            _matchManagerService = gameManagerService;
        }

        [HttpPost("getActiveMatches")]
        public IActionResult GetActiveMatches([FromBody] GameType game)
        {
            var activeMatches = _matchManagerService.GetMatches(game);

            return Ok(activeMatches);
        }

        [HttpPost("getMatchData")]
        public IActionResult GetMatchData(GetMatchDataRequestDto data)
        {
            if (!Enum.TryParse<GameType>(data.Game, true, out var game))
            {
                return BadRequest($"Invalid game type: {game}");
            }

            var activeMatches = _matchManagerService.GetMatch(game, data.MatchId);

            return Ok(activeMatches);
        }
    }
}
