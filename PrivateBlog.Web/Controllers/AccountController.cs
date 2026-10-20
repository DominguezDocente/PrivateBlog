using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.DTOs.Account;
using PrivateBlog.Web.Services.Abstractions;
using MySignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace PrivateBlog.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUsersService _usersService;
        private readonly INotyfService _notyfService;

        public AccountController(IUsersService usersService, INotyfService notyfService)
        {
            _usersService = usersService;
            _notyfService = notyfService;
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
                Response<MySignInResult> result = await _usersService.LoginAsync(dto);

                if (result.IsSuccess)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Email o contrasela incorrectos");
                _notyfService.Error("Email o contraseña incorrectos");
            }
            else
            {
                _notyfService.Error("Debe ajustar los errores de validación");
            }

            return View(dto);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _usersService.LogoutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}
