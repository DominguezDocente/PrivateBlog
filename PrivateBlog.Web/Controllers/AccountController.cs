using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Application.Contracts.Authentication;
using PrivateBlog.Application.Exceptions;
using PrivateBlog.Application.UseCases.Account.Commands.Login;
using PrivateBlog.Application.UseCases.Account.Commands.Logout;
using PrivateBlog.Application.Utils.Mediator;
using PrivateBlog.Web.DTOs.Account;

namespace PrivateBlog.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMediator _mediator;
        private readonly INotyfService _notifyService;

        public AccountController(IMediator mediator, INotyfService notifyService)
        {
            _mediator = mediator;
            _notifyService = notifyService;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                AccountSignInResult result = await _mediator.Send(new LoginCommand
                {
                    Email = model.Email.Trim(),
                    Password = model.Password,
                    RememberMe = model.RememberMe,
                });

                if (result.Succeeded)
                {
                    _notifyService.Success("Sesión iniciada.");

                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return LocalRedirect(model.ReturnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }

                if (result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "Cuenta bloqueada. Intente más tarde.");
                    return View(model);
                }

                ModelState.AddModelError(string.Empty, "Correo o contraseña incorrectos.");
                return View(model);
            }
            catch (CustomValidationException vex)
            {
                foreach (string err in vex.ValidationErrors)
                {
                    ModelState.AddModelError(string.Empty, err);
                }

                return View(model);
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _mediator.Send(new LogoutCommand());
                _notifyService.Success("Sesión cerrada.");
            }
            catch (Exception ex)
            {
                _notifyService.Error(ex.Message);
            }

            return RedirectToAction(nameof(Login));
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult UpdateUser()
        {
            return View();
        }
    }
}
