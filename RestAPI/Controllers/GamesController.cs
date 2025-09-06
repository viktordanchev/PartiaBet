using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllGames()
        {
            var games = await _gamesService.GetAllAsync();

            return Ok(games);
        }
    }
}
