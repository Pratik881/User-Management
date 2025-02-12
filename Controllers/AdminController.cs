
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserManagementSystem.Data;
using UserManagementSystem.Models;

namespace UserManagementSystem.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }


        public async Task<IActionResult> UserList()
        {
           var users= _userManager.Users.ToList();
            return View(users);
        }

        public async Task<IActionResult> AssignRole(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var roles = _roleManager.Roles.ToList(); // Get all roles
            var userRoles = await _userManager.GetRolesAsync(user); // Get roles assigned to the user

            var model = new AssignRoleViewModel
            {
                UserId = userId,
                UserName = user.UserName,
                Roles = roles.Select(role => new RoleModel
                {
                    RoleName = role.Name,
                    IsAssigned = userRoles.Contains(role.Name)
                }).ToList()
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> AssignRole(AssignRoleViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound();
            }

            foreach (var role in model.Roles)
            {
                if (role.IsAssigned)
                {
                    // Assign role if it isn't assigned yet
                    if (!await _userManager.IsInRoleAsync(user, role.RoleName))
                    {
                        await _userManager.AddToRoleAsync(user, role.RoleName);
                    }
                }
                else
                {
                    // Remove role if it is assigned
                    if (await _userManager.IsInRoleAsync(user, role.RoleName))
                    {
                        await _userManager.RemoveFromRoleAsync(user, role.RoleName);
                    }
                }
            }

            return RedirectToAction("UserList"); // Redirect back to the user list
        }


    }

}
