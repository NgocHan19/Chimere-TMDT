using Microsoft.AspNetCore.Mvc;

namespace TMDT.Areas.Admin.Controllers
{
    public class BranchController : Controller
    {
        public IActionResult Branch()
        {
            return View("~/Areas/Admin/Views/Branch/Branch.cshtml");
        }
    }
}
