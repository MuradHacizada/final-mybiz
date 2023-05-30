using FinalExam.Models;
using FinalExam.Utilities.UserRoles;
using FinalExam.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinalExam.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {

            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM newUser)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser user = new AppUser
            {
                Name = newUser.Name,
                Surname = newUser.Surname,
                Email = newUser.Email,
                UserName = newUser.Username
            };
            IdentityResult result = await _userManager.CreateAsync(user, newUser.Password);
            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            await _signInManager.SignInAsync(user, false);

            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }
        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM user, string? ReturUrl)
        {
            if (!ModelState.IsValid) { return View(); }
            AppUser existed = await _userManager.FindByEmailAsync(user.UsernameOreEmail);
            if (existed == null)
            {
                existed = await _userManager.FindByNameAsync(user.UsernameOreEmail);
                if (existed == null)
                {
                    ModelState.AddModelError(string.Empty, "Username Or Email is wrong");
                    return View();
                }
            }
            var result = await _signInManager.PasswordSignInAsync(existed, user.Password, user.IsRemember, true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "you locked!");
                return View();
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Password is wrong");
                return View();
            }
            if (ReturUrl != null)
            {
                Redirect(ReturUrl);
            }
            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }
        public async Task<IActionResult> CreateRoles()
        {
            foreach (var role in Enum.GetValues(typeof(UserRole)))
            {
                if (!(await _roleManager.RoleExistsAsync(role.ToString())))
                {
                    await _roleManager.CreateAsync(new IdentityRole { Name = role.ToString() });
                }
            }
            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }
    }
}
