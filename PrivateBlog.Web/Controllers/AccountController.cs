using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.DTOs;
using PrivateBlog.Web.Services;

namespace PrivateBlog.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly INotyfService _notifyService;
        private readonly IUsersService _usersService;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public AccountController(INotyfService notifyService, IUsersService usersService, IMapper mapper, IEmailService emailService)
        {
            _notifyService = notifyService;
            _usersService = usersService;
            _mapper = mapper;
            _emailService = emailService;
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
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> UpdateUser()
        {
            User user = await _usersService.GetUserAsync(User.Identity.Name);

            if (user == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<AccountUserDTO>(user));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> UpdateUser(AccountUserDTO dto)
        {
            if (ModelState.IsValid)
            {
                var test = await _usersService.UpdateUserAsync(dto);
                return RedirectToAction("Index", "Home");
            }

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
            try
            {
                if (ModelState.IsValid)
                {
                    User user = await _usersService.GetUserAsync(User.Identity.Name);
                    if (user != null)
                    {
                        bool isCorrectPassword = await _usersService.CheckPasswordAsync(user, dto.CurrentPassword);

                        if (!isCorrectPassword)
                        {
                            _notifyService.Error("Contaseña Incorrecta");
                            return View();
                        }

                        string resetToken = await _usersService.GeneratePasswordResetTokenAsync(user);
                        IdentityResult result = await _usersService.ResetPasswordAsync(user, resetToken, dto.NewPassword);

                        if (result.Succeeded)
                        {
                            _notifyService.Success("Contaseña actualizada con éxito");
                            return RedirectToAction("Index", "Home");
                        }

                        _notifyService.Error($"Ha ocurrido un error al intantar cambiar la contraseña");

                        ViewBag.Message = $"Error cambiando la contraseña: {result.Errors}";

                        return View(dto);
                    }

                    _notifyService.Error("Ha ocurrido un error al intantar cambiar la contraseña");
                    return View();
                }

                _notifyService.Error("Debe ajustar los errores de validación");
                return View();
            }
            catch (Exception ex)
            {
                _notifyService.Error("Ocurrió un error al actualizar la contraseña. Por favor intente de nuevo");
                return View();
            }
        }

        [HttpGet]
        public IActionResult RecoverPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordDTO dto)
        {
            if (ModelState.IsValid)
            {
                User? user = await _usersService.GetUserAsync(dto.Email);
                if (user is null)
                {
                    _notifyService.Error("Debe revisar que esté ingresando el correo electrónico correcto, modificarlo, o comunicarse con el Administrador si está seguro de que el correo está correcto");
                    return View(dto);
                }

                string myToken = await _usersService.GeneratePasswordResetTokenAsync(user);
                string link = Url.Action("ResetPassword", "Account", new { token = myToken }, protocol: HttpContext.Request.Scheme)!;

                Response<object> emailResponse = await _emailService.SendResetPasswordEmailAsync(user.Email!, "Para establecer una nueva contraseña haga clic en el siguiente enlace:", link);

                if (!emailResponse.IsSuccess)
                {
                    _notifyService.Error("Ha ocurrido un error. Intente más tarde");
                    return View(dto);
                }

                _notifyService.Success($"Las instrucciones para el cambio de contraseña han sido enviadas a su email ({user.Email})");
                return RedirectToAction(nameof(Login));
            }

            _notifyService.Error("Debe ajustar los errores de validación");
            return View(dto);
        }

        [HttpGet]
        public IActionResult ResetPassword(string token)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO dto)
        {
            try
            {
                User user = await _usersService.GetUserAsync(dto.UserName);
                if (user != null)
                {
                    IdentityResult result = await _usersService.ResetPasswordAsync(user, dto.Token, dto.Password);
                    if (result.Succeeded)
                    {
                        _notifyService.Success("Contaseña actualizada con éxito");
                        return View();
                    }

                    string errors = string.Join(", ", result.Errors.Select(e => e.Description).ToList());
                    errors = errors.Replace("Invalid token.", "El Email ingresado no coincide.");
                    ViewBag.Message = $"Error cambiando la contraseña: {errors} \n Debe revisar que esté ingresando el correo electrónico correcto, modificarlo, o comunicarse con el Administrador si está seguro de que el correo está correcto";
                    _notifyService.Error("El Email ingresado no coincide.");
                    return RedirectToAction(nameof(Login));
                }

                _notifyService.Error("Usuario no encontrado.");
                return View(dto);
            }
            catch (Exception ex)
            {
                _notifyService.Error("Ha ocurrido un error. Intente más tarde.");
                return View(dto);
            }
        }
    }
}
