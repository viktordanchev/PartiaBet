using AutoMapper;
using Core.Enums;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestAPI.Dtos.Match;
using System.Security.Claims;

namespace RestAPI.Controllers
{
    [ApiController]
    [Route("api/matches")]
    public class MatchesController : Controller
    {
        private readonly IMatchService _matchService;
        private readonly IMapper _mapper;

        public MatchesController(IMatchService matchService, IMapper mapper)
        {
            _matchService = matchService;
            _mapper = mapper;
        }

        [HttpPost("getActiveMatches")]
        public async Task<IActionResult> GetActiveMatches([FromBody] GameType gameType)
        {
            var activeMatches = await _matchService.GetActiveMatchesAsync(gameType);
            var activeMatchesDto = _mapper.Map<IEnumerable<MatchDto>>(activeMatches);

            return Ok(activeMatchesDto);
        }

        [HttpPost("getMatch")]
        [Authorize]
        public async Task<IActionResult> GetMatch([FromBody] Guid matchId)
        {
            var match = await _matchService.GetMatchAsync(matchId);
            var matchDto = _mapper.Map<MatchDto>(match);

            return Ok(matchDto);
        }

        [HttpGet("getMatchCountdown")]
        [Authorize]
        public async Task<IActionResult> GetMatchCountdown()
        {
            var playerId = User.FindFirstValue("Id");

            var timeLeft = await _matchService.GetMatchAutoEndTimeRemainingAsync(Guid.Parse(playerId));

            return Ok(timeLeft);
        }
    }
}
