using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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

        public AccountController(INotyfService notifyService, IUsersService usersService, IMapper mapper)
        {
            _notifyService = notifyService;
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
            User user = await _usersService.GetUserAsync(User.Identity.Name);

            if (user == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<AccountUserDTO>(user));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUser(AccountUserDTO dto)
        {
            if (ModelState.IsValid)
            {
                var test = await _usersService.UpdateUserAsync(dto);
                return RedirectToAction("Index", "Home");
            }

            return View(dto);
        }
    }
}
