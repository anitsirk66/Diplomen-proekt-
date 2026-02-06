using BookstoreProjectData.Entities;
using BookstoreWebApp.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System.Net.WebSockets;

namespace BookstoreWebApp.Controllers
{
    [AllowAnonymous]
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

        [Authorize(Roles = "Admin")]
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

        [HttpGet]
        public IActionResult Register()
        {
            var model = new UserRegisterViewModel();

            return View(model);
        }

        [HttpPost]

        public async Task<IActionResult> Register(UserRegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var client = new User()
            {
                Email = model.Email,
                UserName = model.UserName
            };

            var result = await userManager.CreateAsync(client, model.Password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(client, "Client");

                return RedirectToAction("Home", "Index");
            }

            foreach(var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        } 

        [HttpGet]
        public IActionResult Login()
        {
            return View(new UserLoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginViewModel model)
        {
            var check = ModelState.IsValid;
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.FindByEmailAsync(model.Email);
            var result = await signInManager.PasswordSignInAsync(
                user.UserName, 
                model.Password, 
                model.RememberMe, 
                false);

            if(!result.Succeeded)
            {
                ModelState.AddModelError("", "Невалиден имейл или парола");
                return View(model);
            }

            if(await userManager.IsInRoleAsync(user, "Admin"))
            {
                TempData["AdminLoggedInMessage"] = "Admin logged in!";
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles ="Admin")]
        [HttpGet]
        public IActionResult CreateAdmin()
        {
            var viewmodel = new UserCreateAdminViewModel();
            var allRoles = roleManager.Roles.ToList();
            viewmodel.RolesList = allRoles.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.Name
            });

            return View(viewmodel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAdmin(UserCreateAdminViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var admin = new User()
            {
                Email = model.Email,
                UserName = model.UserName
            };

            var result = await userManager.CreateAsync(admin, model.Password);

            if(result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Admin");

                return RedirectToAction("Index", "Home");
            }

            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }
            return View(model);

        }

    }
}
