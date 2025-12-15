using AutoMapper;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using RestAPI.Dtos.Game;

namespace RestAPI.Controllers
{
    [ApiController]
    [Route("api/games")]
    public class GamesController : Controller
    {
        private readonly IGamesService _gamesService;
        private readonly IMapper _mapper;

        public GamesController(IGamesService gamesService, IMapper mapper)
        {
            _gamesService = gamesService;
            _mapper = mapper;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllGames()
        {
            var games = await _gamesService.GetAllAsync();
            var gamesDto = _mapper.Map<IEnumerable<GameDto>>(games);

            return Ok(gamesDto);
        }

        [HttpPost("getGameData")]
        public async Task<IActionResult> GetGameData([FromBody] string game)
        {
            var gameData = await _gamesService.GetGameAsync(game);

            if(gameData == null)
            {
                return NotFound("Game not found");
            }

            var gameDataDto = _mapper.Map<GameDto>(gameData);

            return Ok(gameDataDto);
        }
    }
}
