using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Core.Attributes;
using PrivateBlog.Web.Core.Pagination;
using PrivateBlog.Web.DTOs.Role;
using PrivateBlog.Web.Services.Abstractions;

namespace PrivateRole.Web.Controllers
{
    public class RolesController : Controller
    {
        private readonly INotyfService _notyfService;
        private readonly IRolesService _rolesService;

        public RolesController(INotyfService notyfService, IRolesService rolesService)
        {
            _notyfService = notyfService;
            _rolesService = rolesService;
        }

        [HttpGet]
        [CustomAuthorize(permissionCode: "roles.show")]
        public async Task<IActionResult> Index([FromQuery] PaginationRequest request)
        {
            Response<PaginationResponse<RoleDTO>> response = await _rolesService.GetPaginationAsync(request);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                return RedirectToAction("Index", "Home");
            }

            return View(response.Result);
        }

        [HttpGet]
        [CustomAuthorize(permissionCode: "roles.create")]
        public async Task<IActionResult> Create()
        {
            Response<List<PermissionsForRoleDTO>> permissionsResponse = await _rolesService.GetPermissionsAsync();
            if (!permissionsResponse.IsSuccess)
            {
                _notyfService.Error(permissionsResponse.Message);
                return RedirectToAction(nameof(Index));
            }

            Response<List<SectionsForRoleDTO>> sectionsResponse = await _rolesService.GetSectionsAsync();
            if (!sectionsResponse.IsSuccess)
            {
                _notyfService.Error(sectionsResponse.Message);
                return RedirectToAction(nameof(Index));
            }

            RoleDTO dto = new RoleDTO
            {
                Permissions = permissionsResponse.Result,
                Sections = sectionsResponse.Result
            };

            return View(dto);
        }

        [HttpPost]
        [CustomAuthorize(permissionCode: "roles.create")]
        public async Task<IActionResult> Create([FromForm] RoleDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notyfService.Error("Debe ajustar los errores de validación");

                Response<List<PermissionsForRoleDTO>> permissionsResponse = await _rolesService.GetPermissionsAsync();
                if (!permissionsResponse.IsSuccess)
                {
                    _notyfService.Error(permissionsResponse.Message);
                    return RedirectToAction(nameof(Index));
                }

                Response<List<SectionsForRoleDTO>> sectionsResponse = await _rolesService.GetSectionsAsync();
                if (!sectionsResponse.IsSuccess)
                {
                    _notyfService.Error(sectionsResponse.Message);
                    return RedirectToAction(nameof(Index));
                }

                dto.Permissions = permissionsResponse.Result;
                dto.Sections = sectionsResponse.Result;

                return View(dto);
            }

            Response<RoleDTO> response = await _rolesService.CreateAsync(dto);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message); 
                
                Response<List<PermissionsForRoleDTO>> permissionsResponse = await _rolesService.GetPermissionsAsync();
                if (!permissionsResponse.IsSuccess)
                {
                    _notyfService.Error(permissionsResponse.Message);
                    return RedirectToAction(nameof(Index));
                }

                Response<List<SectionsForRoleDTO>> sectionsResponse = await _rolesService.GetSectionsAsync();
                if (!sectionsResponse.IsSuccess)
                {
                    _notyfService.Error(sectionsResponse.Message);
                    return RedirectToAction(nameof(Index));
                }

                dto.Permissions = permissionsResponse.Result;
                dto.Sections = sectionsResponse.Result;

                return View(dto);
            }

            _notyfService.Success(response.Message);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [CustomAuthorize(permissionCode: "roles.update")]
        public async Task<IActionResult> Edit([FromRoute] Guid id)
        {
            Response<RoleDTO> response = await _rolesService.GetOneAsync(id);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                return RedirectToAction(nameof(Index));
            }

            return View(response.Result);
        }

        [HttpPost]
        [CustomAuthorize(permissionCode: "roles.update")]
        public async Task<IActionResult> Edit([FromForm] RoleDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notyfService.Error("Debe ajustar los errores de validación");

                Response<List<PermissionsForRoleDTO>> permissionsResponse = await _rolesService.GetPermissionsAsync();
                if (!permissionsResponse.IsSuccess)
                {
                    _notyfService.Error(permissionsResponse.Message);
                    return RedirectToAction(nameof(Index));
                }

                Response<List<SectionsForRoleDTO>> sectionsResponse = await _rolesService.GetSectionsAsync();
                if (!sectionsResponse.IsSuccess)
                {
                    _notyfService.Error(sectionsResponse.Message);
                    return RedirectToAction(nameof(Index));
                }

                dto.Permissions = permissionsResponse.Result;
                dto.Sections = sectionsResponse.Result;

                return View(dto);
            }

            Response<RoleDTO> response = await _rolesService.UpdateAsync(dto);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);

                Response<List<PermissionsForRoleDTO>> permissionsResponse = await _rolesService.GetPermissionsAsync();
                if (!permissionsResponse.IsSuccess)
                {
                    _notyfService.Error(permissionsResponse.Message);
                    return RedirectToAction(nameof(Index));
                }

                Response<List<SectionsForRoleDTO>> sectionsResponse = await _rolesService.GetSectionsAsync();
                if (!sectionsResponse.IsSuccess)
                {
                    _notyfService.Error(sectionsResponse.Message);
                    return RedirectToAction(nameof(Index));
                }

                dto.Permissions = permissionsResponse.Result;
                dto.Sections = sectionsResponse.Result;

                return View(dto);
            }

            _notyfService.Success(response.Message);
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [CustomAuthorize(permissionCode: "roles.delete")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Response response = await _rolesService.DeleteAsync(id);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
            }
            else
            {
                _notyfService.Success(response.Message);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
