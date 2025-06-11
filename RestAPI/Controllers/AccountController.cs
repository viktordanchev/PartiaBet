using Microsoft.AspNetCore.Mvc;

namespace RestAPI.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
