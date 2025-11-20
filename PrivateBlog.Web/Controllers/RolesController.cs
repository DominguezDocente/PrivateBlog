using AspNetCoreHero.ToastNotification.Abstractions;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Core.Attributes;
using PrivateBlog.Web.Core.Pagination;
using PrivateBlog.Web.DTOs;
using PrivateRole.Web.Services.Abtractions;
using System.Runtime.CompilerServices;

namespace PrivateBlog.Web.Controllers
{
    public class RolesController : Controller
    {
        private readonly IRolesService _rolesService;
        private readonly INotyfService _notyfService;

        public RolesController(IRolesService rolesService, INotyfService notyfService)
        {
            _rolesService = rolesService;
            _notyfService = notyfService;
        }

        [HttpGet]
        [CustomAuthorize(permission: "showRoles", module: "Roles")]
        public async Task<IActionResult> Index([FromQuery] PaginationRequest request)
        {
            Response<PaginationResponse<PrivateBlogRoleDTO>> response = await _rolesService.GetPaginatedListAsync(request);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                return RedirectToAction("Index", "Home");
            }

            return View(response.Result);
        }

        [HttpGet]
        [CustomAuthorize(permission: "createRoles", module: "Roles")]
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

            PrivateBlogRoleDTO dto = new PrivateBlogRoleDTO
            {
                Permissions = permissionsResponse.Result,
                Sections = sectionsResponse.Result
            };

            return View(dto);
        }

        [HttpPost]
        [CustomAuthorize(permission: "createRoles", module: "Roles")]
        public async Task<IActionResult> Create(PrivateBlogRoleDTO dto)
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

            Response<PrivateBlogRoleDTO> createResponse = await _rolesService.CreateAsync(dto);
            if (createResponse.IsSuccess)
            { 
                 _notyfService.Success(createResponse.Message);
                 return RedirectToAction(nameof(Index));
            }

            _notyfService.Error(createResponse.Message);

            Response<List<PermissionsForRoleDTO>> permissionsResponse2 = await _rolesService.GetPermissionsAsync();
            if (!permissionsResponse2.IsSuccess)
            {
                _notyfService.Error(permissionsResponse2.Message);
                return RedirectToAction(nameof(Index));
            }

            Response<List<SectionsForRoleDTO>> sectionsResponse2 = await _rolesService.GetSectionsAsync();
            if (!sectionsResponse2.IsSuccess)
            {
                _notyfService.Error(sectionsResponse2.Message);
                return RedirectToAction(nameof(Index));
            }

            dto.Permissions = permissionsResponse2.Result;
            dto.Sections = sectionsResponse2.Result;
            return View(dto);
        }


        [HttpGet]
        [CustomAuthorize(permission: "updateRoles", module: "Roles")]
        public async Task<IActionResult> Edit(Guid id)
        {
            Response<PrivateBlogRoleDTO> response = await _rolesService.GetOneAsync(id);
            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                return RedirectToAction(nameof(Index));
            }

            return View(response.Result);
        }

        [HttpPost]
        [CustomAuthorize(permission: "updateRoles", module: "Roles")]
        public async Task<IActionResult> Edit(PrivateBlogRoleDTO dto)
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

            Response<PrivateBlogRoleDTO> updateResponse = await _rolesService.EditAsync(dto);
            if (updateResponse.IsSuccess)
            {
                _notyfService.Success(updateResponse.Message);
                return RedirectToAction(nameof(Index));
            }

            _notyfService.Error(updateResponse.Message);

            Response<List<PermissionsForRoleDTO>> permissionsResponse2 = await _rolesService.GetPermissionsAsync();

            if (!permissionsResponse2.IsSuccess)
            {
                _notyfService.Error(permissionsResponse2.Message);
                return RedirectToAction(nameof(Index));
            }

            Response<List<SectionsForRoleDTO>> sectionsResponse2 = await _rolesService.GetSectionsAsync();
            if (!sectionsResponse2.IsSuccess)
            {
                _notyfService.Error(sectionsResponse2.Message);
                return RedirectToAction(nameof(Index));
            }

            dto.Permissions = permissionsResponse2.Result;
            dto.Sections = sectionsResponse2.Result;
            return View(dto);
        }

        [HttpPost]
        [CustomAuthorize("deleteRoles", "Roles")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Response<object> response = await _rolesService.DeleteAsync(id);

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
