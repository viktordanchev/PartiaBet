using AutoMapper;
using Core.DTOs.Responses;
using Core.Enums;
using Core.Games.Enums;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestAPI.Dtos.Match;

namespace RestAPI.Controllers
{
    [ApiController]
    [Route("api/matches")]
    public class MatchesController : Controller
    {
        private readonly IMatchService _matchService;
        private readonly IMatchTimer _matchTimerManagere;
        private readonly IMapper _mapper;

        public MatchesController(IMatchService matchService, IMatchTimer matchTimerManagere, IMapper mapper)
        {
            _matchService = matchService;
            _mapper = mapper;
            _matchTimerManagere = matchTimerManagere;
        }

        [HttpPost("getActiveMatches")]
        public async Task<IActionResult> GetActiveMatches([FromBody] GameType gameType)
        {
            var activeMatches = await _matchService.GetActiveMatchesAsync(gameType);
            var activeMatchesDto = _mapper.Map<IEnumerable<MatchDto>>(activeMatches);

            return Ok(activeMatchesDto);
        }

        [HttpPost("getMatchData")]
        [Authorize]
        public async Task<IActionResult> GetMatchData([FromBody] Guid matchId)
        {
            var match = await _matchService.GetMatchAsync(matchId);
            var matchDto = _mapper.Map<MatchDto>(match);

            return Ok(matchDto);
        }

        [HttpGet("getSkins")]
        [Authorize]
        public IActionResult GetSkins()
        {
            var skins = new List<PieceSkinDto>
            {
                new PieceSkinDto
                {
                    Type = PieceType.Bishop,
                    White = "https://partiabetstorage.blob.core.windows.net/chess-pieces/classic-white-bishop.svg",
                    Black = "https://partiabetstorage.blob.core.windows.net/chess-pieces/classic-black-bishop.svg"
                },
                new PieceSkinDto
                {
                    Type = PieceType.King,
                    White = "https://partiabetstorage.blob.core.windows.net/chess-pieces/classic-white-king.svg",
                    Black = "https://partiabetstorage.blob.core.windows.net/chess-pieces/classic-black-king.svg"
                },
                new PieceSkinDto
                {
                    Type = PieceType.Knight,
                    White = "https://partiabetstorage.blob.core.windows.net/chess-pieces/classic-white-knight.svg",
                    Black = "https://partiabetstorage.blob.core.windows.net/chess-pieces/classic-black-knight.svg"
                },
                new PieceSkinDto
                {
                    Type = PieceType.Pawn,
                    White = "https://partiabetstorage.blob.core.windows.net/chess-pieces/classic-white-pawn.svg",
                    Black = "https://partiabetstorage.blob.core.windows.net/chess-pieces/classic-black-pawn.svg"
                },
                new PieceSkinDto
                {
                    Type = PieceType.Queen,
                    White = "https://partiabetstorage.blob.core.windows.net/chess-pieces/classic-white-queen.svg",
                    Black = "https://partiabetstorage.blob.core.windows.net/chess-pieces/classic-black-queen.svg"
                },
                new PieceSkinDto
                {
                    Type = PieceType.Rook,
                    White = "https://partiabetstorage.blob.core.windows.net/chess-pieces/classic-white-rook.svg",
                    Black = "https://partiabetstorage.blob.core.windows.net/chess-pieces/classic-black-rook.svg"
                }
            };

            return Ok(skins);
        }
    }
}
