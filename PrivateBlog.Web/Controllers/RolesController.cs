using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Application.Contracts.Pagination;
using PrivateBlog.Application.Contracts.Security;
using PrivateBlog.Application.UseCases.Roles.Commands.CreateRole;
using PrivateBlog.Application.UseCases.Roles.Commands.DeleteRole;
using PrivateBlog.Application.UseCases.Roles.Commands.UpdateRole;
using PrivateBlog.Application.UseCases.Roles.Queries.GetPermissionsByModule;
using PrivateBlog.Application.UseCases.Roles.Queries.GetRoleById;
using PrivateBlog.Application.UseCases.Roles.Queries.GetRolesList;
using PrivateBlog.Application.Utilities.Mediator;
using PrivateBlog.Web.DTOs.Roles;
using PrivateBlog.Web.Security;

namespace PrivateBlog.Web.Controllers
{
    public class RolesController : Controller
    {
        private readonly INotyfService _notifyService;
        private readonly IMediator _mediator;

        public RolesController(INotyfService notifyService, IMediator mediator)
        {
            _notifyService = notifyService;
            _mediator = mediator;
        }

        public async Task<IActionResult> Index([FromQuery] int page = 1,
                                               [FromQuery] int pageSize = PaginationRequest.DEFAULT_PAGE_SIZE,
                                               [FromQuery] string? nameFilter = null)
        {
            try
            {
                PaginationRequest paginationRequest = new PaginationRequest(page, pageSize);

                GetRolesListQuery query = new GetRolesListQuery
                {
                    Pagination = paginationRequest,
                    NameFilter = nameFilter,
                };

                PaginationResponse<RoleListItemDTO> list = await _mediator.Send(query);

                RolesIndexViewModel viewModel = new RolesIndexViewModel
                {
                    List = list,
                    FilterName = nameFilter ?? string.Empty,
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _notifyService.Error($"Error al cargar los roles: {ex.Message}");
                return View(new RolesIndexViewModel
                {
                    List = PaginationResponse<RoleListItemDTO>.Create([], 0, new PaginationRequest(1, pageSize)),
                });
            }
        }

        [HttpGet]
        [RequirePermission(PermissionCodesCatalog.CREATE_ROLES)]
        public async Task<IActionResult> Create()
        {
            IReadOnlyList<PermissionModuleGroupDTO> modules = await _mediator.Send(new GetPermissionsByModuleQuery());

            CreateRoleDTO dto = new CreateRoleDTO
            {
                PermissionModules = modules,
            };

            return View(dto);
        }

        [HttpPost]
        [RequirePermission(PermissionCodesCatalog.CREATE_ROLES)]
        public async Task<IActionResult> Create(CreateRoleDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _notifyService.Error("Debe corregir los errores de validación.");
                    dto.PermissionModules = await _mediator.Send(new GetPermissionsByModuleQuery());
                    return View(dto);
                }

                CreateRoleCommand command = new CreateRoleCommand
                {
                    Name = dto.Name,
                    PermissionIds = dto.PermissionIds,
                };

                await _mediator.Send(command);
                _notifyService.Success("Rol creado exitosamente.");
            }
            catch (Exception ex)
            {
                _notifyService.Error($"Error al crear el rol: {ex.Message}");
                dto.PermissionModules = await _mediator.Send(new GetPermissionsByModuleQuery());
                return View(dto);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [RequirePermission(PermissionCodesCatalog.EDIT_ROLES)]
        public async Task<IActionResult> Edit([FromRoute] Guid id)
        {
            try
            {
                RoleDetailDTO role = await _mediator.Send(new GetRoleByIdQuery { Id = id });
                IReadOnlyList<PermissionModuleGroupDTO> modules = await _mediator.Send(new GetPermissionsByModuleQuery());

                EditRoleDTO editDto = new EditRoleDTO
                {
                    Id = role.Id,
                    Name = role.Name,
                    PermissionIds = role.PermissionIds,
                    PermissionModules = modules,
                };

                return View(editDto);
            }
            catch (Exception ex)
            {
                _notifyService.Error($"Error al cargar el rol: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [RequirePermission(PermissionCodesCatalog.EDIT_ROLES)]
        public async Task<IActionResult> Edit(EditRoleDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _notifyService.Error("Debe corregir los errores de validación.");
                    dto.PermissionModules = await _mediator.Send(new GetPermissionsByModuleQuery());
                    return View(dto);
                }

                UpdateRoleCommand command = new UpdateRoleCommand
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    PermissionIds = dto.PermissionIds,
                };

                await _mediator.Send(command);
                _notifyService.Success("Rol actualizado exitosamente.");
            }
            catch (Exception ex)
            {
                _notifyService.Error($"Error al actualizar el rol: {ex.Message}");
                dto.PermissionModules = await _mediator.Send(new GetPermissionsByModuleQuery());
                return View(dto);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [RequirePermission(PermissionCodesCatalog.DELETE_ROLES)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                await _mediator.Send(new DeleteRoleCommand { Id = id });
                _notifyService.Success("Rol eliminado exitosamente.");
            }
            catch (Exception ex)
            {
                _notifyService.Error($"Error al eliminar el rol: {ex.Message}");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}