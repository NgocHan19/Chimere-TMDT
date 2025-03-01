using Microsoft.AspNetCore.Mvc;

namespace TMDT.Controllers
{
    public class WishlistController : Controller
    {
        public IActionResult WishlistEmpty()
        {
            return View("~/Views/Wishlist/Wishlist-Empty.cshtml");
        }
    }
}
