using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.DTOs.Account;
using PrivateBlog.Web.Services.Abstractions;
using PrivateBlog.Web.Services.Implementations;
using MySignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace PrivateBlog.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly INotyfService _notyfService;
        private readonly IUsersService _usersService;
        private readonly IMapper _mapper;

        public AccountController(IAccountService accountService, INotyfService notyfService, IUsersService usersService, IMapper mapper)
        {
            _accountService = accountService;
            _notyfService = notyfService;
            _usersService = usersService;
            _mapper = mapper;
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
                Response<MySignInResult> result = await _accountService.LoginAsync(dto);

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
            await _accountService.LogoutAsync();
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UpdateUser()
        {
            User? user = null;

            string email = User?.Identity?.Name;

            if (string.IsNullOrEmpty(email))
            {
                _notyfService.Error("El email es requerido");
                return RedirectToAction("Index", "Home");
            }

            Response<User> userResponse = await _usersService.GetUserByEmailAsync(email);

            if (!userResponse.IsSuccess)
            {
                _notyfService.Error(userResponse.Message);
                return RedirectToAction("Index", "Home");
            }

            user = userResponse.Result;

            AccountUserDTO dto = _mapper.Map<AccountUserDTO>(user);

            return View(dto);            
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateUser(AccountUserDTO dto)
        {
            Response<AccountUserDTO> result = await _accountService.UpdateAccountUserAsync(dto);

            if (result.IsSuccess)
            {
                _notyfService.Success(result.Message);
            }
            else
            {
                _notyfService.Error(result.Message);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
