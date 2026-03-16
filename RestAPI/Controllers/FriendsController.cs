using AutoMapper;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestAPI.Dtos.User;
using System.Security.Claims;

namespace RestAPI.Controllers
{
    [ApiController]
    [Route("api/friends")]
    public class FriendsController : Controller
    {
        private readonly IFriendshipService _friendshipService;
        private readonly IMapper _mapper;

        public FriendsController(IFriendshipService friendshipService,
            IMapper mapper)
        {
            _friendshipService = friendshipService;
            _mapper = mapper;
        }

        [HttpGet("getFriends")]
        [Authorize]
        public async Task<IActionResult> GetFriends()
        {
            var playerId = Guid.Parse(User.FindFirstValue("Id"));

            var friends = await _friendshipService.GetFriendsAsync(playerId);
            var friendsDto = _mapper.Map<IEnumerable<FriendDto>>(friends);

            return Ok(friendsDto);
        }

        [HttpPost("getPlayers")]
        public async Task<IActionResult> GetPlayers([FromBody] string username)
        {
            var users = await _friendshipService.GetAllUsersAsync(username);
            var friendsDto = _mapper.Map<IEnumerable<FriendDto>>(users);

            return Ok(users);
        }

        //[HttpPost("getPlayer")]
        //public async Task<IActionResult> GetPlayer([FromBody] Guid playerId)
        //{
        //    var user = await _friendshipService.GetUserProfileAsync(userId);
        //
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //
        //    var userDto = _mapper.Map<UserProfileDto>(user);
        //
        //    return Ok(userDto);
        //}
    }
}
