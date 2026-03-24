using AutoMapper;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestAPI.Dtos.Friendship;
using System.Security.Claims;

namespace RestAPI.Controllers
{
    [ApiController]
    [Authorize]
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
        public async Task<IActionResult> GetFriends()
        {
            var playerId = Guid.Parse(User.FindFirstValue("Id"));

            var friends = await _friendshipService.GetFriendsAsync(playerId);
            var friendsDto = _mapper.Map<IEnumerable<FriendDto>>(friends);

            return Ok(friendsDto);
        }

        [HttpPost("searchPlayers")]
        public async Task<IActionResult> SearchPlayers([FromBody] string username)
        {
            var users = await _friendshipService.GetAllUsersAsync(username);
            var friendsDto = _mapper.Map<IEnumerable<FriendDto>>(users);

            return Ok(users);
        }

        [HttpPost("getPlayer")]
        public async Task<IActionResult> GetPlayer([FromBody] Guid playerId)
        {
            var userId = Guid.Parse(User.FindFirstValue("Id"));

            var player = await _friendshipService.GetPlayerProfileAsync(userId, playerId);
        
            if (player == null)
                return NotFound();

            var playerDto = _mapper.Map<PlayerDataDto>(player);
        
            return Ok(playerDto);
        }

        [HttpPost("sendFriendRequest")]
        public async Task<IActionResult> SendFriendRequest([FromBody] Guid receiverId)
        {
            var userId = Guid.Parse(User.FindFirstValue("Id"));

            await _friendshipService.SendFriendRequestAsync(userId, receiverId);

            return NoContent();
        }

        [HttpPost("acceptFriendRequest")]
        public async Task<IActionResult> AcceptFriendRequest([FromBody] Guid senderId)
        {
            var userId = Guid.Parse(User.FindFirstValue("Id"));

            await _friendshipService.AcceptFriendRequestAsync(senderId, userId);

            return NoContent();
        }

        [HttpPost("cancelFriendRequest")]
        public async Task<IActionResult> CancelFriendRequest([FromBody] Guid senderId)
        {
            var userId = Guid.Parse(User.FindFirstValue("Id"));

            await _friendshipService.RemoveFriendAsync(senderId, userId);

            return NoContent();
        }

        [HttpGet("getPendingFriendRequests")]
        public async Task<IActionResult> GetPendingFriendRequests()
        {
            var userId = Guid.Parse(User.FindFirstValue("Id"));



            return Ok();
        }

        [HttpPost("removeFriend")]
        public async Task<IActionResult> RemoveFriend([FromBody] Guid friendId)
        {
            var userId = Guid.Parse(User.FindFirstValue("Id"));

            await _friendshipService.RemoveFriendAsync(userId, friendId);

            return NoContent();
        }
    }
}
