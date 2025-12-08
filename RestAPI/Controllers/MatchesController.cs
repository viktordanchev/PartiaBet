using Core.DTOs.Responses;
using Core.Enums;
using Games.Chess.Enums;
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
