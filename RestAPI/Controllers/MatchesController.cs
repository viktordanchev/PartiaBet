using Core.Enums;
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
        public IActionResult GetActiveMatches([FromBody] GameType gameId)
        {
            var activeMatches = _matchManagerService.GetMatches(gameId);

            return Ok(activeMatches);
        }
    }
}
