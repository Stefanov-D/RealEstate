using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RealEstate.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")] // Only accessible by Admins
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
