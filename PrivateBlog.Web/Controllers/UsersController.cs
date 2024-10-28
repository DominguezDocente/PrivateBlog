using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Core.Pagination;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.DTOs;
using PrivateBlog.Web.Helpers;
using PrivateBlog.Web.Services;

namespace PrivateBlog.Web.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly ICombosHelper _combosHelper;
        private readonly INotyfService _notifyService;
        private readonly IUsersService _usersService;

        public UsersController(ICombosHelper combosHelper, INotyfService notifyService, IUsersService usersService)
        {
            _combosHelper = combosHelper;
            _notifyService = notifyService;
            _usersService = usersService;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] int? RecordsPerPage,
                                               [FromQuery] int? Page,
                                               [FromQuery] string? Filter)
        {
            PaginationRequest request = new PaginationRequest
            {
                RecordsPerPage = RecordsPerPage ?? 15,
                Page = Page ?? 1,
                Filter = Filter
            };

            Response<PaginationResponse<User>> response = await _usersService.GetListAsync(request);
            return View(response.Result);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            UserDTO dto = new UserDTO
            {
                PrivateBlogRoles = await _combosHelper.GetComboProvateBlogRolesAsync(),
            };

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _notifyService.Error("Debe ajustar los errores de validación");
                    dto.PrivateBlogRoles = await _combosHelper.GetComboProvateBlogRolesAsync();
                    return View(dto);
                }

                Response<User> response = await _usersService.CreateAsync(dto);

                if (response.IsSuccess)
                {
                    _notifyService.Success(response.Message);
                    return RedirectToAction(nameof(Index));
                }

                _notifyService.Error(response.Message);
                dto.PrivateBlogRoles = await _combosHelper.GetComboProvateBlogRolesAsync();
                return View(dto);
            }
            catch (Exception ex)
            {
                dto.PrivateBlogRoles = await _combosHelper.GetComboProvateBlogRolesAsync();
                return View(dto);
            }
        }



    }
}
