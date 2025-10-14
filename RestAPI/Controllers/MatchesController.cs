using Core.Enums;
using Interfaces.Games;
using Microsoft.AspNetCore.Mvc;

namespace RestAPI.Controllers
{
    [ApiController]
    [Route("api/matches")]
    public class MatchesController : Controller
    {
        private readonly IMatchManagerService _gameManagerService;

        public MatchesController(IMatchManagerService gameManagerService)
        {
            _gameManagerService = gameManagerService;
        }

        [HttpPost("getActiveMatches")]
        public IActionResult GetActiveMatches([FromBody] GameType gameType)
        {
            var activeMatches = _gameManagerService.GetMatches(gameType);

            return Ok(activeMatches);
        }
    }
}
