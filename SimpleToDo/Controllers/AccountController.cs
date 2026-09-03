using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimpleToDo.Application.Features.Users.Commands;
using SimpleToDo.Application.Interfaces;
using SimpleToDo.Domain.Entities;
using SimpleToDo.Infrastructure.Identity;
using SimpleToDo.Web.ViewModels.Auth;

namespace SimpleToDo.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISender _mediator;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,ISender mediator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mediator = mediator;
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
                return RedirectToAction("Index", "Project");
            }
            ModelState.AddModelError(string.Empty, "Invalid username or password");
            return View(model);
        }
        public IActionResult Register() 
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model, string? returnUrl)
        {
            if (!ModelState.IsValid) 
            {
                return View(model);
            }
            var user = new ApplicationUser
            {
                FullName = model.FullName,
                Email = model.Email,
                UserName = model.Email
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }
            await _mediator.Send(new CreateUserCommand(user.Id,user.FullName, user.Email));
            await _signInManager.SignInAsync(user, isPersistent: true);
            if (!string.IsNullOrEmpty(returnUrl) && returnUrl != null)
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
