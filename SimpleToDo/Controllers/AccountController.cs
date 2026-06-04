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
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string? returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }
            var result = await _signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, true, false);
            if (result.Succeeded) 
            {
                return RedirectToAction(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
