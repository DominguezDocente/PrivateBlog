using AspNetCoreHero.ToastNotification.Abstractions;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Application.UseCases.Sections.Commands.CreateSection;
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
                GetSectionsListQuery command = new GetSectionsListQuery();
                list = await _mediator.Send(command);
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
        public async Task<IActionResult> Create(CreateSectionDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _notifyService.Error("Debe corregir los erores de validación.");
                    return View(dto);
                }

                CreateSectionCommand command = new CreateSectionCommand { Name = dto.Name };
                Guid newSectionId = await _mediator.Send(command);
                _notifyService.Success("Sección creada con éxito");
            }
            catch(Exception ex)
            {
                _notifyService.Error(ex.Message);
            }
            
            return RedirectToAction("Index");            
        }
    }
}
