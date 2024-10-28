using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Web.DTOs;
using PrivateBlog.Web.Services;

namespace PrivateBlog.Web.Controllers
{
	public class AccountController : Controller
	{
		private readonly INotyfService _notifyService;
		private readonly IUsersService _usersService;

        public AccountController(IUsersService usersService, INotyfService notifyService)
        {
            _usersService = usersService;
            _notifyService = notifyService;
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
					return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos");
                _notifyService.Error("Email o contraseña incorrectos");

				return View(dto);
            }

			return View(dto);
		}

		[HttpGet]
		public async Task<IActionResult> Logout()
		{
			await _usersService.LogoutAsync();
			return RedirectToAction(nameof(Login));
		}

		[HttpGet]
		public IActionResult NotAuthorized()
		{
			return View();
		}

    }
}
