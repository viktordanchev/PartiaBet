using Core.Enums;
using Interfaces.Games;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public IActionResult GetMatchData([FromBody] Guid matchId)
        {
            var activeMatches = _matchManagerService.GetMatch(matchId);

            return Ok(activeMatches);
        }
    }
}
