using Microsoft.AspNetCore.Mvc;

namespace TMDT.Controllers
{
    public class WishlistController : Controller
    {
        public IActionResult Wishlist()
        {
            return View("~/Views/Wishlist/Wishlist.cshtml");
        }

        //public IActionResult Empty()
        //{
        //    return View("~/Views/Wishlist/Wishlist-Empty.cshtml");
        //}
    }
}
