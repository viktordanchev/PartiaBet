using AutoMapper;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using RestAPI.Dtos.Games;

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
        public IActionResult GetAllGames()
        {
            var games = _gamesService.GetAll();
            var gamesDto = _mapper.Map<IEnumerable<GameDto>>(games);

            return Ok(gamesDto);
        }

        [HttpPost("getGameData")]
        public IActionResult GetGameData([FromBody] string game)
        {
            var gameData = _gamesService.GetGame(game);

            if (gameData == null)
            {
                return NotFound("Game not found");
            }

            var gameDataDto = _mapper.Map<GameDto>(gameData);

            return Ok(gameDataDto);
        }
    }
}
