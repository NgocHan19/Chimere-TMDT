using Microsoft.AspNetCore.Mvc;

namespace TMDT.Areas.Admin.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Employee()
        {
            return View("~/Areas/Admin/Views/Employee/Employee.cshtml");
        }
    }
}
