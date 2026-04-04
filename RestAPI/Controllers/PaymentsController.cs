using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RestAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/payments")]
    public class PaymentsController : Controller
    {
        [HttpPost("deposit")]
        public async Task<IActionResult> CreateDeposit([FromBody] DepositRequestDto data)
        {

            return Ok();
        }
    }
}
