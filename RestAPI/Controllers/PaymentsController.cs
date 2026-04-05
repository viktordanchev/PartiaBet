using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestAPI.Dtos.Payments;

namespace RestAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/payments")]
    public class PaymentsController : Controller
    {
        [HttpPost("deposit")]
        public async Task<IActionResult> MakeDeposit([FromBody] DepositRequestDto data)
        {


            return Ok();
        }
    }
}
