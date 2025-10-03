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
        public async Task<IActionResult> GetGameDetails([FromBody] string game)
        {
            var gameData = await _gamesService.GetDetailsAsync(game);

            if(gameData == null)
            {
                return NotFound("Game not found");
            }

            return Ok(gameData);
        }
    }
}
