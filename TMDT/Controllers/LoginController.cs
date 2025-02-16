using Microsoft.AspNetCore.Mvc;

namespace TMDT.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
