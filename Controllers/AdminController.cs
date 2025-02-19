using Azure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserManagementSystem.Data;
using UserManagementSystem.Models;
using UserManagementSystem.ViewModels;
using System.Linq;

namespace UserManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
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
            var users = _userManager.Users.ToList();
            var userRoles = new Dictionary<string, List<string>>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRoles.Add(user.Id, roles.ToList());
            }

            ViewBag.UserRoles = userRoles;
            return View(users);
        }

       
        public async Task<IActionResult> EditUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = _roleManager.Roles.Select(r => r.Name).ToList();

            var model = new EditUserViewModel
            {
                UserId = userId,
                FullName= user.FullName,
                Email = user.Email,
                AllRoles = allRoles,
                SelectedRoles = userRoles.ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound();
            }

            user.FullName = model.FullName;
            user.Email = model.Email;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Something went wrong.");
                return View(model);
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            var rolesToRemove = currentRoles.Except(model.SelectedRoles).ToList();
            var rolesToAdd = model.SelectedRoles.Except(currentRoles).ToList();

           
            foreach (var role in rolesToRemove)
            {
                await _userManager.RemoveFromRoleAsync(user, role);
            }

           
            foreach (var role in rolesToAdd)
            {
                await _userManager.AddToRoleAsync(user, role);
            }

            return RedirectToAction("UserList");
        }

        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            await _userManager.DeleteAsync(user);
            return RedirectToAction("UserList");
        }
    }
}
