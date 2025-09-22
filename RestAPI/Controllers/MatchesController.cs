using Core.Interfaces.Games;
using Microsoft.AspNetCore.Mvc;

namespace RestAPI.Controllers
{
    [ApiController]
    [Route("api/matches")]
    public class MatchesController : Controller
    {
        private readonly IGameManagerService _gameManagerService;

        public MatchesController(IGameManagerService gameManagerService)
        {
            _gameManagerService = gameManagerService;
        }

        [HttpPost("getActiveMatches")]
        public IActionResult GetActiveMatches([FromBody] int gameId)
        {
            var activeMatches = _gameManagerService.GetMatches(gameId);

            return Ok(activeMatches);
        }
    }
}
