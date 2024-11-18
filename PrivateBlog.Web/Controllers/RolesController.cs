using AspNetCoreHero.ToastNotification.Abstractions;
using t = Azure;
using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Core.Attributes;
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
        public async Task<IActionResult> Index([FromQuery] int? RecordsPerPage,
                                               [FromQuery] int? Page,
                                               [FromQuery] string? Filter)
        {
            PaginationRequest paginationRequest = new PaginationRequest
            {
                RecordsPerPage = RecordsPerPage ?? 15,
                Page = Page ?? 1,
                Filter = Filter,
            };

            Response<PaginationResponse<PrivateBlogRole>> response = await _rolesService.GetListAsync(paginationRequest);

            return View(response.Result);
        }

        [HttpGet]
        [CustomAuthorize(permission: "createRoles", module: "Roles")]
        public async Task<IActionResult> Create()
        {
            Response<IEnumerable<Permission>> response = await _rolesService.GetPermissionsAsync();

            if (!response.IsSuccess)
            {
                _notifyService.Error(response.Message);
                return RedirectToAction(nameof(Index));
            }

            PrivateBlogRoleDTO dto = new PrivateBlogRoleDTO
            {
                Permissions = response.Result.Select(p => new PermissionForDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Module = p.Module,
                }).ToList()
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

                Response<IEnumerable<Permission>> response1 = await _rolesService.GetPermissionsAsync();

                dto.Permissions = response1.Result.Select(p => new PermissionForDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Module = p.Module,
                }).ToList();

                return View(dto);
            }

            Response<PrivateBlogRole> createResponse = await _rolesService.CreateAsync(dto); 
            
            if (createResponse.IsSuccess)
            {
                _notifyService.Success(createResponse.Message);
                return RedirectToAction(nameof(Index));
            }

            _notifyService.Error(createResponse.Message);

            Response<IEnumerable<Permission>> response = await _rolesService.GetPermissionsAsync();

            dto.Permissions = response.Result.Select(p => new PermissionForDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Module = p.Module,
            }).ToList();

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

        [HttpPost]
        [CustomAuthorize(permission: "updateRoles", module: "Roles")]
        public async Task<IActionResult> Edit(PrivateBlogRoleDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notifyService.Error("Debe ajustar los errores de validación");

                Response<IEnumerable<PermissionForDTO>> permissionsByRoleResponse = await _rolesService.GetPermissionsByRoleAsync(dto.Id);
                dto.Permissions = permissionsByRoleResponse.Result.ToList();

                return View(dto);
            }

            Response<PrivateBlogRole> editResponse = await _rolesService.EditAsync(dto);

            if (editResponse.IsSuccess)
            {
                _notifyService.Success(editResponse.Message);
                return RedirectToAction(nameof(Index));
            }

            _notifyService.Error(editResponse.Message);

            Response<IEnumerable<PermissionForDTO>> permissionsByRoleResponse2 = await _rolesService.GetPermissionsByRoleAsync(dto.Id);
            dto.Permissions = permissionsByRoleResponse2.Result.ToList();

            return View(dto);
        }

    }
}
