using Microsoft.AspNetCore.Mvc;

namespace TMDT.Controllers
{
    public class DetailController : Controller
    {
        public IActionResult Detail()
        {
            return View("~/Views/Product/Search.cshtml");
        }
    }
}
