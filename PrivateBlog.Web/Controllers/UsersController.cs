using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Core.Attributes;
using PrivateBlog.Web.Core.Pagination;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.DTOs;
using PrivateBlog.Web.Helpers;
using PrivateBlog.Web.Helpers.Abstractions;
using PrivateBlog.Web.Services;
using PrivateBlog.Web.Services.Abtractions;

namespace PrivateBlog.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersService _usersService;
        private readonly INotyfService _notifyService;
        private readonly ICombosHelper _combosHelper;
        private readonly IMapper _mapper;

        public UsersController(IUsersService sectionsService, INotyfService notifyService, ICombosHelper combosHelper, IMapper mapper)
        {
            _usersService = sectionsService;
            _notifyService = notifyService;
            _combosHelper = combosHelper;
            _mapper = mapper;
        }

        [HttpGet]
        [CustomAuthorize(permission: "showUsers", module: "Usuarios")]
        public async Task<IActionResult> Index([FromQuery] PaginationRequest request)
        {
            Response<PaginationResponse<UserDTO>> response = await _usersService.GetPaginatedListAsync(request);
            return View(response.Result);
        }

        [HttpGet]
        [CustomAuthorize(permission: "createUsers", module: "Usuarios")]
        public async Task<IActionResult> Create()
        {
            IEnumerable<SelectListItem> items = await _combosHelper.GetComboRoles();

            UserDTO dto = new UserDTO
            {
                PrivateBlogRoles = items,
            };

            return View(dto);
        }

        [HttpPost]
        [CustomAuthorize(permission: "createUsers", module: "Usuarios")]
        public async Task<IActionResult> Create(UserDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notifyService.Error("Debe ajustar los errores de validación");
                dto.PrivateBlogRoles = await _combosHelper.GetComboRoles();
                return View(dto);
            }

            Response<UserDTO> response = await _usersService.CreateAsync(dto);

            if (!response.IsSuccess)
            {
                _notifyService.Error(response.Message);
                dto.PrivateBlogRoles = await _combosHelper.GetComboRoles();
                return View(dto);
            }

            _notifyService.Success(response.Message);
            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        [CustomAuthorize(permission: "updateUsers", module: "Usuarios")]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (Guid.Empty.Equals(id))
            {
                return NotFound();
            }

            User user = await _usersService.GetUserByIdAsync(id);

            if (user is null)
            {
                return NotFound();
            }

            UserDTO dto = _mapper.Map<UserDTO>(user);
            dto.PrivateBlogRoles = await _combosHelper.GetComboRoles();

            return View(dto);
        }

        [HttpPost]
        [CustomAuthorize(permission: "updateUsers", module: "Usuarios")]
        public async Task<IActionResult> Edit(UserDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notifyService.Error("Debe ajustar los errores de validación");
                dto.PrivateBlogRoles = await _combosHelper.GetComboRoles();
                return View(dto);
            }

            Response<UserDTO> response = await _usersService.EditAsync(dto);

            if (!response.IsSuccess)
            {
                _notifyService.Error(response.Message);
                dto.PrivateBlogRoles = await _combosHelper.GetComboRoles();
                return View(dto);
            }

            _notifyService.Success(response.Message);
            return RedirectToAction(nameof(Index));
        }
    }
}