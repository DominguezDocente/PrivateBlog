using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Application.UseCases.Sections.Commands.ActivateSection;
using PrivateBlog.Application.UseCases.Sections.Commands.CreateSection;
using PrivateBlog.Application.UseCases.Sections.Commands.DeactivateSection;
using PrivateBlog.Application.UseCases.Sections.Commands.DeleteSection;
using PrivateBlog.Application.UseCases.Sections.Commands.UpdateSection;
using PrivateBlog.Application.UseCases.Sections.Queries.GetSectionById;
using PrivateBlog.Application.UseCases.Sections.Queries.GetSectionsList;
using PrivateBlog.Application.Utils.Mediator;
using PrivateBlog.Web.DTOs.Sections;

namespace PrivateBlog.Web.Controllers
{
    public class SectionsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly INotyfService _notifyService;

        public SectionsController(IMediator mediator, INotyfService notifyService)
        {
            _mediator = mediator;
            _notifyService = notifyService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<SectionListItemDTO> list = new List<SectionListItemDTO>();
            try
            {
                GetSectionsListQuery query = new GetSectionsListQuery();
                list = await _mediator.Send(query);
            }
            catch (Exception ex)
            {
                _notifyService.Error(ex.Message);
            }

            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSectionDTO createSectionDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _notifyService.Error("Debe corregir los errores de validación.");
                    return View(createSectionDto);
                }

                CreateSectionCommand createCommand = new CreateSectionCommand { Name = createSectionDto.Name };
                await _mediator.Send(createCommand);
                _notifyService.Success("Sección creada con éxito");
            }
            catch(Exception ex)
            {
                _notifyService.Error(ex.Message);
            }
            
            return RedirectToAction("Index");            
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                SectionDetailDTO? section = await _mediator.Send(new GetSectionByIdQuery { Id = id });

                if (section is null)
                {
                    _notifyService.Error("No se encontró la sección.");
                    return RedirectToAction(nameof(Index));
                }

                EditSectionDTO editSectionDto = new EditSectionDTO
                {
                    Id = section.Id,
                    Name = section.Name,
                    IsActive = section.IsActive,
                };

                return View(editSectionDto);
            }
            catch (Exception ex)
            {
                _notifyService.Error(ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditSectionDTO editSectionDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _notifyService.Error("Debe corregir los errores de validación.");
                    return View(editSectionDto);
                }

                UpdateSectionCommand updateCommand = new UpdateSectionCommand
                {
                    Id = editSectionDto.Id,
                    Name = editSectionDto.Name,
                    IsActive = editSectionDto.IsActive,
                };

                await _mediator.Send(updateCommand);
                _notifyService.Success("Sección actualizada con éxito");
            }
            catch (Exception ex)
            {
                _notifyService.Error(ex.Message);
                return View(editSectionDto);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _mediator.Send(new DeleteSectionCommand { Id = id });
                _notifyService.Success("Sección eliminada con éxito");
            }
            catch (Exception ex)
            {
                _notifyService.Error(ex.Message);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Activate(Guid id)
        {
            try
            {
                await _mediator.Send(new ActivateSectionCommand { Id = id });
                _notifyService.Success("Sección activada");
            }
            catch (Exception ex)
            {
                _notifyService.Error(ex.Message);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Deactivate(Guid id)
        {
            try
            {
                await _mediator.Send(new DeactivateSectionCommand { Id = id });
                _notifyService.Success("Sección desactivada");
            }
            catch (Exception ex)
            {
                _notifyService.Error(ex.Message);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
