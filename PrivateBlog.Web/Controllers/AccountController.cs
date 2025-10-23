﻿using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.DTOs;
using PrivateBlog.Web.Services.Abtractions;
using System.Threading.Tasks;

namespace PrivateBlog.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUsersService _usersService;
        private readonly IMapper _mapper;
        private readonly INotyfService _notyfService;

        public AccountController(IUsersService usersService, IMapper mapper, INotyfService notyfService)
        {
            _usersService = usersService;
            _mapper = mapper;
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
                Response<Microsoft.AspNetCore.Identity.SignInResult> result = await _usersService.LoginAsync(dto);

                if (result.IsSuccess)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos");
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

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> UpdateUser()
        {
            User user = await _usersService.GetUserByEmailasync(User.Identity.Name);

            if (user is null)
            {
                return NotFound();
            }

            return View(_mapper.Map<AccountUserDTO>(user));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateUser(AccountUserDTO dto)
        {
            if (ModelState.IsValid)
            {
                Response<AccountUserDTO> result = await _usersService.UpdateUserAsync(dto);

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

            _notyfService.Error("Debe ajustar lo errores de validación");
            return View(dto);
        }
    }
}
