using API_Arch_Core.DataBaseObjects.Areas.Identity;
using API_Arch_ViewModel.MVC_ViewModesl.Areas.Auth;
using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPIArchitecture.MVC_Controller
{
    public class AuthMVCController : Controller
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly INotyfService _notyf;

        public AuthMVCController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager, INotyfService notyf)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _notyf = notyf;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            AppUser? user = null;

            // Case 1 — Login by phone number (10 digits)
            if (!string.IsNullOrEmpty(model.UserName) &&
                model.UserName.All(char.IsDigit) &&
                model.UserName.Length == 10)
            {
                user = await _userManager.Users
                    .FirstOrDefaultAsync(x => x.PhoneNumber == model.UserName || x.UserName == model.UserName);
            }
            // Case 2 — Login by username
            else if (!string.IsNullOrWhiteSpace(model.UserName))
            {
                user = await _userManager.FindByNameAsync(model.UserName);
            }
            // Case 3 — Login by email
            else
            {
                user = await _userManager.FindByEmailAsync(model.UserName);
            }


            if (user == null)
            {
                //return Unauthorized(new
                //{
                //    message = "Invalid email, phone number, or username"
                //});
                _notyf.Error("Invalid email, phone number, or username");
                return RedirectToAction("Index", "Home");

            }


            if (!user.IsActive)
            {
                //return Unauthorized(new { message = "Account inactive. Contact admin for activation." });

                _notyf.Error("Account inactive. Contact admin for activation.");
                return RedirectToAction("Index", "Home");
            }

            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains("SystemAdmin"))
            {
                return Unauthorized(new
                {
                    message = "Only admins can log in here"
                });
            }

            // Correct password sign-in
            var result = await _signInManager.PasswordSignInAsync(
                user.UserName,
                model.Password,
                isPersistent: false,
                lockoutOnFailure: true
            );

            if (result.Succeeded && user.IsActive)
            {
                TempData["successMessage"] = "Login successful";
                return RedirectToAction("Index", "Home");

            }



            var failedAttempts = await _userManager.GetAccessFailedCountAsync(user);

            if (result.IsLockedOut)
            {
                return Unauthorized(new { message = "Account locked. Try again later." });
            }

            return Unauthorized(new
            {
                message = "Invalid password",
                accessFailedCount = failedAttempts
            });
        }

        public async Task<IActionResult> Logout()
        {
            TempData["alertMessage"] = "You loged out.";
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
