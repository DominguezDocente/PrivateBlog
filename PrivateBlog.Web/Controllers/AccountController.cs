using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.DTOs;
using PrivateBlog.Web.Services;
using System.Threading.Tasks;

namespace PrivateBlog.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUsersService _usersService;
        private readonly IMapper _mapper;
        private readonly INotyfService _notifyService;

        public AccountController(IUsersService usersService, IMapper mapper, INotyfService notifyService)
        {
            _usersService = usersService;
            _mapper = mapper;
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
            }

            return View(dto);
        }

        [HttpGet]
        [Route("Errors/{statusCode:int}")]
        public IActionResult Error(int statusCode)
        {
            string errorMessage = "Ha ocurrido un error";

            switch (statusCode)
            {
                case StatusCodes.Status401Unauthorized:
                    errorMessage = "Debes iniciar sesión";
                    break;

                case StatusCodes.Status403Forbidden:
                    errorMessage = "No tienes permiso para estar aquí";
                    break;

                case StatusCodes.Status404NotFound:
                    errorMessage = "La página que estás intentando acceder no existe";
                    break;
            }

            ViewBag.ErrorMessage = errorMessage;

            return View(statusCode);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _usersService.LogoutAsync();
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UpdateUser()
        {
            User user = await _usersService.GetUserAsync(User.Identity.Name);

            if (user is null)
            {
                return NotFound();
            }

            return View(_mapper.Map<AccountUserDTO>(user));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateUser(AccountUserDTO dto)
        {
            if (ModelState.IsValid)
            {
                int affecrtedRows = await _usersService.UpdateUserAsync(dto);

                if (affecrtedRows > 0)
                {
                    _notifyService.Success("Datos actualizados con éxito");
                }
                else
                {
                    _notifyService.Error("Ha ocurrido un error al intentar actualizar los datos.");
                }

                return RedirectToAction("Index", "Home");
            }

            _notifyService.Error("Debe ajustar los errores de validación");
            return View(dto);
        }
    }
}
