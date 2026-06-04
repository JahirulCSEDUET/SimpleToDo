using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimpleToDo.Infrastructure.Identity;
using SimpleToDo.Web.ViewModels.Auth;

namespace SimpleToDo.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, false);
            if (result.Succeeded) 
            { 
                if (!string.IsNullOrEmpty(returnUrl) && returnUrl != null)
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError(string.Empty, "Ivalid username or password");
            return View(model);
        }

    }
}
