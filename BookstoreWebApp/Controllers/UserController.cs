using BookstoreProjectData.Entities;
using BookstoreWebApp.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System.Net.WebSockets;

namespace BookstoreWebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserController(UserManager<User> _userManagaer, SignInManager<User> _signInManager, RoleManager<IdentityRole> _roleManager)
        {
            userManager = _userManagaer;
            signInManager = _signInManager;
            roleManager = _roleManager;
        }

        [AllowAnonymous]
        public async Task<IActionResult> SeedRoles()
        {
            string[] roles = { "Admin", "Client" };
            foreach (var role in roles)
            {
                if(!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
            return Content("Roles seeded (created if mising).");
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var model = new UserRegisterViewModel
            {
                Roles = new List<string> { "Admin", "Client" }
            };

            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = new User()
            {
                Email = model.Email,
                UserName = model.UserName
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                if (model.Role == "Admin" || model.Role == "Client") {
                    await userManager.AddToRoleAsync(user, model.Role); }

                return RedirectToAction("Login", "User");
            }
            foreach(var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }
            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View(new UserLoginViewModel());  
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure:false);

            return View(model);
        }
    }
}
