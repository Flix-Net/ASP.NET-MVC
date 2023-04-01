using EatAndDrink.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace EatAndDrink.Controllers
{
    public class UserController : Controller
    {
        UserManager<IdentityUser> _userManager;
        RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<IdentityUser> userManager,  RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Index()
        {
            List<UserWithRoleViewModel> userWithRolesViewModels = new List<UserWithRoleViewModel>();
            List<IdentityUser> users = _userManager.Users.ToList();
            foreach (IdentityUser user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Count > 0)
                {
                    userWithRolesViewModels.Add(new UserWithRoleViewModel() { Id = user.Id, Email = user.Email, EmailConfirmed = user.EmailConfirmed, Role = roles.First() });
                }
                else
                {
                    userWithRolesViewModels.Add(new UserWithRoleViewModel() { Id = user.Id, Email = user.Email, EmailConfirmed = user.EmailConfirmed, Role = "" });
                }
 
            }

            return View(userWithRolesViewModels);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string Id)
        {
            IdentityUser user = await _userManager.FindByIdAsync(Id);
            ViewData["roles"] = _roleManager.Roles.ToList();
            var roles = await _userManager.GetRolesAsync(user);

            UserWithRoleViewModel viewModel;

            if (roles.Count > 0)
            {
                viewModel = new UserWithRoleViewModel() { Id = user.Id, Email = user.Email, EmailConfirmed = user.EmailConfirmed, Role = roles.First() };
            }
            else
            {
                viewModel = new UserWithRoleViewModel() { Id = user.Id, Email = user.Email, EmailConfirmed = user.EmailConfirmed };
            }

            return View(viewModel);
        }

        public async Task<IActionResult> Update(UserWithRoleViewModel viewModel)
        {
            if(viewModel == null)
            {
                return View("Error");
            }

            IdentityUser user = await _userManager.FindByIdAsync(viewModel.Id);
            await _userManager.SetEmailAsync(user, viewModel.Email);

            var roles = await _userManager.GetRolesAsync(user);

            await _userManager.RemoveFromRolesAsync(user, roles);

            if(viewModel.Role.Trim() != "") 
            {
                await _userManager.AddToRoleAsync(user, viewModel.Role);
            }

            user.EmailConfirmed = viewModel.EmailConfirmed;
            await _userManager.UpdateAsync(user);

            return RedirectToAction("Index");
        }
    }
}
