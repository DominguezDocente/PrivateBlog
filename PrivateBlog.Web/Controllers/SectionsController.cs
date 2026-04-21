using AspNetCoreHero.ToastNotification.Abstractions;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Application.Contracts.Pagination;
using PrivateBlog.Application.UseCases.Sections.Commands.ActivateSection;
using PrivateBlog.Application.UseCases.Sections.Commands.CreateSection;
using PrivateBlog.Application.UseCases.Sections.Commands.DeactivateSeccion;
using PrivateBlog.Application.UseCases.Sections.Commands.DeleteSection;
using PrivateBlog.Application.UseCases.Sections.Commands.UpdateSection;
using PrivateBlog.Application.UseCases.Sections.Queries.GetSectionById;
using PrivateBlog.Application.UseCases.Sections.Queries.GetSectionsList;
using PrivateBlog.Application.Utilities.Mediator;
using PrivateBlog.Web.DTOs.Sections;

namespace PrivateBlog.Web.Controllers
{
    public class SectionsController : Controller
    {
        private readonly INotyfService _notifyService;
        private readonly IMediator _mediator;

        public SectionsController(INotyfService notifyService, IMediator mediator)
        {
            _notifyService = notifyService;
            _mediator = mediator;
        }

        public async Task<IActionResult> Index([FromQuery] int page = 1,
                                               [FromQuery] int pageSize = PaginationRequest.DEFAULT_PAGE_SIZE,
                                               [FromQuery] string? nameFilter = null,
                                               [FromQuery] bool? isActiveFilter = null)
        {
            try
            {
                PaginationRequest paginationRequest = new PaginationRequest(page, pageSize);
                GetSectionsListQuery query = new GetSectionsListQuery
                {
                    Pagination = paginationRequest,
                    NameFilter = nameFilter,
                    IsActiveFilter = isActiveFilter
                };

                PaginationResponse<SectionListItemDTO> list = await _mediator.Send(query);

                SectionsIndexViewModel viewModel = new SectionsIndexViewModel
                {
                    List = list,
                    FilterName = nameFilter ?? string.Empty,
                    FilterIsActive = isActiveFilter
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _notifyService.Error($"Error al cargar las secciones: {ex.Message}");
                return View(new List<SectionListItemDTO>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            CreateSectionCommand command = new CreateSectionCommand { Name = "123" };
            Guid newSectionId = await _mediator.Send(command);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSectionDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _notifyService.Error("Debe correjir los errores de validación.");
                    return View(dto);
                }

                CreateSectionCommand command = new CreateSectionCommand { Name = dto.Name };
                Guid newSectionId = await _mediator.Send(command);
                _notifyService.Success("Sección creada exitosamente.");
            }
            catch (Exception ex)
            {
                _notifyService.Error($"Error al crear la sección: {ex.Message}");
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] Guid id)
        {
            try
            {
                SectionDetailDTO dto = await _mediator.Send(new GetSectionByIdQuery { Id = id });

                EditSectionDTO editDto = new EditSectionDTO
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    IsActive = dto.IsActive
                };

                return View(editDto);
            }
            catch (Exception ex)
            {
                _notifyService.Error($"Error al cargar la sección: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditSectionDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _notifyService.Error("Debe correjir los errores de validación.");
                    return View(dto);
                }

                UpdateSectionCommand command = new UpdateSectionCommand
                {
                    Name = dto.Name,
                    Id = dto.Id,
                    IsActive = dto.IsActive
                };

                await _mediator.Send(command);
                _notifyService.Success("Sección actualizada exitosamente.");
            }
            catch (Exception ex)
            {
                _notifyService.Error($"Error al editar la sección: {ex.Message}");
                return View(dto);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                await _mediator.Send(new DeleteSectionCommand { Id = id });
                _notifyService.Success("Sección eliminada exitosamente.");
            }

            catch (Exception ex)
            {
                _notifyService.Error($"Error al eliminar la sección: {ex.Message}");
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Activate([FromRoute] Guid id)
        {
            try
            {
                await _mediator.Send(new ActivateSectionCommand { Id = id });
                _notifyService.Success("Sección activada exitosamente.");
            }

            catch (Exception ex)
            {
                _notifyService.Error($"Error al activar la sección: {ex.Message}");
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Deactivate([FromRoute] Guid id)
        {
            try
            {
                await _mediator.Send(new DeactivateSeccionCommand { Id = id });
                _notifyService.Success("Sección desactivada exitosamente.");
            }

            catch (Exception ex)
            {
                _notifyService.Error($"Error al desactivar la sección: {ex.Message}");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
