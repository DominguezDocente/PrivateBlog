using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.DTOs;
using PrivateBlog.Web.Helpers;
using PrivateBlog.Web.Services;

namespace PrivateBlog.Web.Controllers
{
	public class AccountController : Controller
	{
		private readonly INotyfService _notifyService;
		private readonly IUsersService _usersService;
        private readonly IConverterHelper _converterHelper;

        public AccountController(IUsersService usersService, INotyfService notifyService, IConverterHelper converterHelper)
        {
            _usersService = usersService;
            _notifyService = notifyService;
            _converterHelper = converterHelper;
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


        [HttpGet]
        public async Task<IActionResult> UpdateUser()
        {
            User? user = await _usersService.GetUserAsync(User.Identity.Name);

            if (user is null)
            {
                return NotFound();
            }

            AccountUserDTO dto = _converterHelper.ToAccountDTO(user);

            return View(dto);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateUser(AccountUserDTO dto)
        {
            if (ModelState.IsValid)
            {
                User user = await _usersService.GetUserAsync(User.Identity.Name);

                user.Document = dto.Document;
                user.FirstName = dto.FirstName;
                user.LastName = dto.LastName;
                user.PhoneNumber = dto.PhoneNumber;

                await _usersService.UpdateUserAsync(user);

                _notifyService.Success("Usuario editado con éxito");

                return RedirectToAction("Dashboard", "Home");
            }

            _notifyService.Error("Debe ajustar los errores de validación.");
            return View(dto);
        }

        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO dto)
        {
            if (ModelState.IsValid)
            {
                User user = await _usersService.GetUserAsync(User.Identity.Name);

                bool isCorrectPassword = await _usersService.CheckPasswordAsync(user, dto.CurrentPassword);

                if (!isCorrectPassword)
                {
                    _notifyService.Error("Contraseña incorrecta");
                    return View();
                }

                string restToken = await _usersService.GeneratePasswordResetTokenAsync(user);
                IdentityResult result = await _usersService.ResetPasswordAsync(user, restToken, dto.NewPassword);

                if (result.Succeeded)
                {
                    _notifyService.Success("Contraseña actualizada con éxito");
                    return RedirectToAction("Dashboard", "Home");
                }

                _notifyService.Error("Ha ocurriso un error, intentole nuevamente");
                return View();
            }

            _notifyService.Error("Debe ajustar los errores de validación");
            return View();
        }

    }
}
