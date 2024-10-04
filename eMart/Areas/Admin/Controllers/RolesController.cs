using eMart.DTO_Models;
using eMart.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace eMart.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = clsRoles.RoleAdmin)]
    public class RolesController : Controller
    {
        public RolesController(UserManager<AppUser> user, RoleManager<IdentityRole> roles)
        {
            _user = user;
            _roles = roles;
        }

        private readonly UserManager<AppUser> _user;
        private readonly RoleManager<IdentityRole> _roles;

        public async Task<IActionResult> Index()
        {
            ViewBag.isAdmin = HttpContext.Session.GetString("isAdmin");
            ViewData["Count"] = HttpContext.Session.GetInt32("count");
            var _users = await _user.Users.ToListAsync();
            return View(_users);
        }

        public async Task<IActionResult> AddRoles(string userId)
        {
            ViewBag.isAdmin = HttpContext.Session.GetString("isAdmin");
            ViewData["Count"] = HttpContext.Session.GetInt32("count");
 
            var user = await _user.FindByIdAsync(userId);
            if (user != null)
            {
                var userRoles = await _user.GetRolesAsync(user);

                var allRoles = await _roles.Roles.ToListAsync();

                if (allRoles != null)
                {
                    var roleList = allRoles.Select(r => new RoleViewModel()
                    {
                        RoleId = r.Id,
                        RoleName = r.Name,
                        UseRole = userRoles.Any(x => x == r.Name)
                    });
                    ViewBag.userName = user.UserName;
                    ViewBag.userId = userId;
                    return View(roleList);
                }
                else
                    return NotFound();
            }

            else
                return NotFound();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRoles(string userId, string jsonRoles)
        {
            var user = await _user.FindByIdAsync(userId);


            List<RoleViewModel> myRoles =
                JsonConvert.DeserializeObject<List<RoleViewModel>>(jsonRoles);

            if (user != null)
            {
                var userRoles = await _user.GetRolesAsync(user);
                if (myRoles != null)
                {
                    foreach (var role in myRoles)
                    {
                        if (role.RoleName != null)
                        {
                            if (userRoles.Any(x => x == role.RoleName.Trim()) && !role.UseRole)
                            {
                                await _user.RemoveFromRoleAsync(user, role.RoleName.Trim());
                            }

                            if (!userRoles.Any(x => x == role.RoleName.Trim()) && role.UseRole)
                            {
                                await _user.AddToRoleAsync(user, role.RoleName.Trim());
                            }
                        }
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            else
                return NotFound();
        }
    }
}
