using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace RestAPI.Controllers
{
    [ApiController]
    [Route("api/games")]
    public class GamesController : Controller
    {
        private readonly IGamesService _gamesService;

        public GamesController(IGamesService gamesService)
        {
            _gamesService = gamesService;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllGames()
        {
            var games = await _gamesService.GetAllAsync();

            return Ok(games);
        }

        [HttpPost("getGameDetails")]
        public async Task<IActionResult> GetGameDetails([FromBody] int gameId)
        {
            var game = await _gamesService.GetDetailsAsync(gameId);

            return Ok(game);
        }
    }
}
