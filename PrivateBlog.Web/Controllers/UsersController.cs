using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Core.Attributes;
using PrivateBlog.Web.Core.Pagination;
using PrivateBlog.Web.DTOs.Section;
using PrivateBlog.Web.DTOs.User;
using PrivateBlog.Web.Helpers.Abstractions;
using PrivateBlog.Web.Services.Abstractions;
using System.Runtime.CompilerServices;

namespace PrivateBlog.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly INotyfService _notyfService;
        private readonly IUsersService _usersService;
        private readonly ISelectListHelper _selectListHelper;

        public UsersController(INotyfService notyfService, IUsersService usersService, ISelectListHelper selectListHelper)
        {
            _notyfService = notyfService;
            _usersService = usersService;
            _selectListHelper = selectListHelper;
        }

        [HttpGet]
        [CustomAuthorize(permissionCode: "users.show")]
        public async Task<IActionResult> Index([FromQuery] PaginationRequest request)
        {
            Response<PaginationResponse<UserDTO>> response = await _usersService.GetPaginationAsync(request);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                return RedirectToAction("Index", "Home");
            }

            return View(response.Result);
        }

        [HttpGet]
        [CustomAuthorize(permissionCode: "users.create")]
        public async Task<IActionResult> Create()
        {
            CreateUserDTO dto = new CreateUserDTO
            {
                PrivateBlogRoles = await _selectListHelper.GetRolesSelectListAsync()
            };

            return View(dto);
        }

        [HttpPost]
        [CustomAuthorize(permissionCode: "users.create")]
        public async Task<IActionResult> Create([FromForm] CreateUserDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notyfService.Error("Debe ajustar los errores de validación");
                dto.PrivateBlogRoles = await _selectListHelper.GetRolesSelectListAsync();
                return View(dto);
            }

            Response<UserDTO> response = await _usersService.CreateAsync(dto);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                dto.PrivateBlogRoles = await _selectListHelper.GetRolesSelectListAsync();
                return View(dto);
            }

            _notyfService.Success(response.Message);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [CustomAuthorize(permissionCode: "users.update")]
        public async Task<IActionResult> Edit([FromRoute] Guid id)
        {
            Response<UserDTO> response = await _usersService.GetOneAsync(id);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                return RedirectToAction(nameof(Index));
            }

            UserDTO dto = response.Result;
            dto.PrivateBlogRoles = await _selectListHelper.GetRolesSelectListAsync();

            return View(dto);
        }

        [HttpPost]
        [CustomAuthorize(permissionCode: "users.update")]
        public async Task<IActionResult> Edit([FromForm] UserDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notyfService.Error("Debe ajustar los errores de validación");
                dto.PrivateBlogRoles = await _selectListHelper.GetRolesSelectListAsync();
                return View(dto);
            }

            Response<UserDTO> response = await _usersService.UpdateUserAsync(dto);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                dto.PrivateBlogRoles = await _selectListHelper.GetRolesSelectListAsync();
                return View(dto);
            }

            _notyfService.Success(response.Message);
            return RedirectToAction(nameof(Index));
        }
    }
}
