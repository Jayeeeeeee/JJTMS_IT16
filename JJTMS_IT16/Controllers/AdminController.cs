using JJTMS_IT16.Data;
using JJTMS_IT16.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JJTMS_IT16.Controllers
{
    // Only accessible by Admins
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Admin/UserList
        public IActionResult UserList()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        // GET: Admin/CreateUser
        public IActionResult CreateUser()
        {
            ViewData["Roles"] = _roleManager.Roles.ToList();
            return View();
        }

        // POST: Admin/CreateUser
        [HttpPost]
        public async Task<IActionResult> CreateUser(string email, string password, string role)
        {
            if (ModelState.IsValid)
            {
                var user = new UserModel { UserName = email, Email = email };
                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    // Assign role to user
                    await _userManager.AddToRoleAsync(user, role);
                    return RedirectToAction("UserList");
                }
                else
                {
                    // Handle errors
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            ViewData["Roles"] = _roleManager.Roles.ToList();
            return View();
        }

        // GET: Admin/EditUserRole/{id}
        public async Task<IActionResult> EditUserRole(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var roles = _roleManager.Roles.ToList();

            var model = new EditUserRoleModel
            {
                UserId = user.Id,
                Email = user.Email,
                Roles = roles,
                SelectedRole = userRoles.FirstOrDefault()
            };

            return View(model);
        }

        // POST: Admin/EditUserRole
        [HttpPost]
        public async Task<IActionResult> EditUserRole(EditUserRoleModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound();
            }

            var currentRoles = await _userManager.GetRolesAsync(user);

            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to remove existing roles");
                return View(model);
            }

            var addResult = await _userManager.AddToRoleAsync(user, model.SelectedRole);
            if (!addResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to add new role");
                return View(model);
            }

            return RedirectToAction("UserList");
        }

        // POST: Admin/DeleteUser/{id}
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                // Handle errors
                ModelState.AddModelError("", "Failed to delete user");
                return RedirectToAction("UserList");
            }

            return RedirectToAction("UserList");
        }
    }
}
