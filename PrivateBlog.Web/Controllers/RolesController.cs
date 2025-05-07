using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Web.Core.Attributes;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Core.Pagination;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.DTOs;
using PrivateBlog.Web.Services;

namespace PrivateBlog.Web.Controllers
{
    public class RolesController : Controller
    {
        private readonly IRolesService _rolesService;
        private readonly INotyfService _notifyService;

        public RolesController(IRolesService rolesService, INotyfService notifyService)
        {
            _rolesService = rolesService;
            _notifyService = notifyService;
        }

        [HttpGet]
        [CustomAuthorize(permission: "showRoles", module: "Roles")]
        public async Task<IActionResult> Index([FromQuery] PaginationRequest request)
        {
            Response<PaginationResponse<PrivateBlogRoleDTO>> response = await _rolesService.GetPaginationAsync(request);
            if (!response.IsSuccess)
            {
                _notifyService.Error(response.Message);
                return RedirectToAction("Index", "Home");
            }
            return View(response.Result);
        }

        [HttpGet]
        [CustomAuthorize(permission: "createRoles", module: "Roles")]
        public async Task<IActionResult> Create()
        {
            Response<List<PermissionDTO>> permissionsResponse = await _rolesService.GetPermissionsAsync();

            if (!permissionsResponse.IsSuccess)
            {
                _notifyService.Error(permissionsResponse.Message);
                return RedirectToAction(nameof(Index));
            }

            Response<List<SectionDTO>> sectionsResponse = await _rolesService.GetSectionsAsync();

            if (!sectionsResponse.IsSuccess)
            {
                _notifyService.Error(sectionsResponse.Message);
                return RedirectToAction(nameof(Index));
            }

            PrivateBlogRoleDTO dto = new PrivateBlogRoleDTO
            {
                Permissions = permissionsResponse.Result.Select(p => new PermissionForRoleDTO 
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Module = p.Module,
                    Selected = false
                }).ToList(),

                Sections = sectionsResponse.Result.Select(p => new SectionForRoleDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Selected = false
                }).ToList(),
            };

            return View(dto);
        }



        [HttpPost]
        [CustomAuthorize(permission: "createRoles", module: "Roles")]
        public async Task<IActionResult> Create(PrivateBlogRoleDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notifyService.Error("Debe ajustar los errores de validación");

                Response<List<PermissionDTO>> permissionResponse1 = await _rolesService.GetPermissionsAsync();
                Response<List<SectionDTO>> sectionsResponse1 = await _rolesService.GetSectionsAsync();

                dto.Permissions = permissionResponse1.Result.Select(p => new PermissionForRoleDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Module = p.Module,
                    Selected = false,
                }).ToList();

                dto.Sections = sectionsResponse1.Result.Select(p => new SectionForRoleDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                }).ToList();

                return View(dto);
            }

            Response<PrivateBlogRoleDTO> createResponse = await _rolesService.CreateAsync(dto);

            if (createResponse.IsSuccess)
            {
                _notifyService.Success(createResponse.Message);
                return RedirectToAction(nameof(Index));
            }

            _notifyService.Error(createResponse.Message);

            Response<List<PermissionDTO>> pemrissionResponse2 = await _rolesService.GetPermissionsAsync();
            Response<List<SectionDTO>> sectionsResponse2 = await _rolesService.GetSectionsAsync();

            dto.Permissions = pemrissionResponse2.Result.Select(p => new PermissionForRoleDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Module = p.Module,
            }).ToList();

            dto.Sections = sectionsResponse2.Result.Select(p => new SectionForRoleDTO
            {
                Id = p.Id,
                Name = p.Name,
            }).ToList();

            return View(dto);
        }


        [HttpPost]
        [CustomAuthorize(permission: "updateRoles", module: "Roles")]
        public async Task<IActionResult> Edit(PrivateBlogRoleDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notifyService.Error("Debe ajustar los errores de validación");

                Response<List<PermissionForRoleDTO>> permissionsByRoleResponse = await _rolesService.GetPermissionsByRoleAsync(dto.Id);
                Response<List<SectionForRoleDTO>> sectionsByRoleResponse = await _rolesService.GetSectionsByRoleAsync(dto.Id);
                dto.Permissions = permissionsByRoleResponse.Result.ToList();
                dto.Sections = sectionsByRoleResponse.Result.ToList();

                return View(dto);
            }

            Response<PrivateBlogRoleDTO> editResponse = await _rolesService.EditAsync(dto);

            if (editResponse.IsSuccess)
            {
                _notifyService.Success(editResponse.Message);
                return RedirectToAction(nameof(Index));
            }

            _notifyService.Error(editResponse.Message);

            Response<List<PermissionForRoleDTO>> permissionsByRoleResponse2 = await _rolesService.GetPermissionsByRoleAsync(dto.Id);
            Response<List<SectionForRoleDTO>> sectionsByRoleResponse2 = await _rolesService.GetSectionsByRoleAsync(dto.Id);
            dto.Permissions = permissionsByRoleResponse2.Result.ToList();
            dto.Sections = sectionsByRoleResponse2.Result.ToList();

            return View(dto);
        }

        [HttpGet]
        [CustomAuthorize(permission: "updateRoles", module: "Roles")]
        public async Task<IActionResult> Edit(int id)
        {
            Response<PrivateBlogRoleDTO> response = await _rolesService.GetOneAsync(id);

            if (!response.IsSuccess)
            {
                _notifyService.Error(response.Message);
                return RedirectToAction(nameof(Index));
            }

            return View(response.Result);
        }
    }
}
