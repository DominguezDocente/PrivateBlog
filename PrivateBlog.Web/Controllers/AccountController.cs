using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Application.UseCases.Account.Commands.ChangePassword;
using PrivateBlog.Application.UseCases.Account.Commands.Login;
using PrivateBlog.Application.UseCases.Account.Commands.Logout;
using PrivateBlog.Application.UseCases.Account.Commands.UpdateProfile;
using PrivateBlog.Application.UseCases.Account.Queries.GetAccountProfile;
using PrivateBlog.Application.Utilities.Mediator;
using PrivateBlog.Web.DTOs.Account;
using System.Security.Claims;

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

        [HttpGet]
        public IActionResult Login([FromQuery] string? returnUrl = null)
        {
            return View(new LoginDTO { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notifyService.Error("Debe corregir los errores de validación.");
                return View(dto);
            }

            try
            {
                LoginCommand command = new LoginCommand
                {
                    UserName = dto.Email,
                    Password = dto.Password,
                    RememberMe = dto.RememberMe,
                };

                AccountSignInResult result = await _mediator.Send(command);

                if (result.Succeeded)
                {
                    _notifyService.Success("Inicio de sesión exitoso.");

                    if (!string.IsNullOrEmpty(dto.ReturnUrl) && Url.IsLocalUrl(dto.ReturnUrl))
                    {
                        return Redirect(dto.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                if (result.IsLockedOut)
                {
                    _notifyService.Error("Su cuenta ha sido bloqueada temporalmente debido a múltiples intentos fallidos de inicio de sesión. Por favor, inténtelo de nuevo más tarde.");
                    return View(dto);
                }

                _notifyService.Error("Usuario o contraseña incorrectos.");
                return View(dto);
            }
            catch (Exception ex)
            {
                _notifyService.Error(ex.Message);
                return View(dto);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Logout()
        {
            try
            {
                await _mediator.Send(new LogoutCommand());
                _notifyService.Success("Cierre de sesión exitoso.");
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                _notifyService.Error(ex.Message);
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return RedirectToAction(nameof(Login));
            }

            try
            {
                EditProfileDTO dto = await BuildProfileDtoAsync(userId);
                return View(dto);
            }
            catch (Exception ex)
            {
                _notifyService.Error(ex.Message);
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Profile(EditProfileDTO dto)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return RedirectToAction(nameof(Login));
            }

            try
            {
                if (!ModelState.IsValid)
                {
                    _notifyService.Error("Debe corregir los errores de validación.");
                    return View(await BuildProfileDtoAsync(userId, dto));
                }

                await _mediator.Send(new UpdateProfileCommand
                {
                    UserId = userId,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    PhoneNumber = dto.PhoneNumber,
                });

                _notifyService.Success("Perfil actualizado exitosamente.");
                return RedirectToAction(nameof(Profile));
            }
            catch (Exception ex)
            {
                _notifyService.Error(ex.Message);
                return View(await BuildProfileDtoAsync(userId, dto));
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View(new ChangePasswordDTO());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO dto)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return RedirectToAction(nameof(Login));
            }

            if (!ModelState.IsValid)
            {
                _notifyService.Error("Debe corregir los errores de validación.");
                return View(dto);
            }

            try
            {
                await _mediator.Send(new ChangePasswordCommand
                {
                    UserId = userId,
                    CurrentPassword = dto.CurrentPassword,
                    NewPassword = dto.NewPassword,
                });

                _notifyService.Success("Contraseña actualizada exitosamente.");
                return RedirectToAction(nameof(Profile));
            }
            catch (Exception ex)
            {
                _notifyService.Error(ex.Message);
                return View(dto);
            }
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View("Forbbiden");
        }

        private async Task<EditProfileDTO> BuildProfileDtoAsync(string userId, EditProfileDTO? posted = null)
        {
            AccountProfileDTO profile = await _mediator.Send(new GetAccountProfileQuery { UserId = userId });

            return new EditProfileDTO
            {
                FirstName = posted?.FirstName ?? profile.FirstName,
                LastName = posted?.LastName ?? profile.LastName,
                Email = profile.Email,
                PhoneNumber = posted?.PhoneNumber ?? profile.PhoneNumber,
                RoleName = profile.RoleName,
            };
        }
    }
}
