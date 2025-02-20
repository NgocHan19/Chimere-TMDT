using Microsoft.AspNetCore.Mvc;

namespace TMDT.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View("~/Views/Account/Login.cshtml");
        }
    }
}
