using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Web.DTOs;
using PrivateBlog.Web.Services;

namespace PrivateBlog.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUsersService _usersService;
        private readonly INotyfService _noty;

        public AccountController(IUsersService usersService, INotyfService noty)
        {
            _usersService = usersService;
            _noty = noty;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _usersService.LoginAsync(dto);
                if (result.Succeeded)
                {
                    if (Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(Request.Query["ReturnUrl"].First());
                    }

                    return RedirectToAction("Dashboard", "Home");
                }

                ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos");
                _noty.Error("Email o contraseña incorrectos");
            }

            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _usersService.LogoutAsync();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult NotAuthorized()
        {
            return View();
        }
    }
}
