using eMart.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eMart.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = clsRoles.RoleAdmin)]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.isAdmin = HttpContext.Session.GetString("isAdmin");
            ViewData["Count"] = HttpContext.Session.GetInt32("count");
            return View();
        }
    }
}
