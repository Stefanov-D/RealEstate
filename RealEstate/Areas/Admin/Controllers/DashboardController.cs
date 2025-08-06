using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Models.Admin;
using RealEstate.Infrastructure.Identity;

namespace RealEstate.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DashboardController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();

            var model = new List<AdminUserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                model.Add(new AdminUserViewModel
                {
                    Id = user.Id,
                    Email = user.Email ?? "",
                    FullName = user.UserName ?? "",
                    Roles = roles.ToList()
                });
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserRole(string userId, string selectedRole)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(selectedRole))
            {
                TempData["Error"] = "Invalid input.";
                return RedirectToAction("Index");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                TempData["Error"] = "User not found.";
                return RedirectToAction("Index");
            }

            var currentRoles = await _userManager.GetRolesAsync(user);

            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRoleAsync(user, selectedRole);

            TempData["SuccessMessage"] = "User role updated successfully!";
            return RedirectToAction("Index");
        }
    }
}
