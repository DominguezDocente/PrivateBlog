using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.Helpers;
using PrivateBlog.Web.Requests;
using PrivateBlog.Web.Services;

namespace PrivateBlog.Web.Controllers
{
    public class SectionsController : Controller
    {
        private readonly ISectionsService _sectionsService;
        public readonly INotyfService _notifyService;

        public SectionsController(ISectionsService sectionsService, INotyfService notifyService)
        {
            _sectionsService = sectionsService;
            _notifyService = notifyService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            Response<List<Section>> response = await _sectionsService.GetListAsync();
            return View(response.Result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Section section)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(section);
                }

                Response<Section> response = await _sectionsService.CreateAsync(section);

                if (response.IsSuccess)
                {
                    _notifyService.Success(response.Message);
                    return RedirectToAction(nameof(Index));
                }

                _notifyService.Error(response.Errors.First());
                return View(response);
            }
            catch (Exception ex)
            {
                return View(section);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            Response<Section> response = await _sectionsService.GetOneAsync(id);

            if (response.IsSuccess) 
            {
                return View(response.Result);
            }

            // TODO: mensaje error
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Section section)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // TODO: mensaje error
                    return View(section);
                }

                Response<Section> response = await _sectionsService.EditAsync(section);

                if (response.IsSuccess)
                {
                    // TODO: mensaje exito
                    return RedirectToAction(nameof(Index));
                }

                // TODO: Mostrar mensaje de error
                return View(response);
            }
            catch (Exception ex)
            {
                // TODO: mensaje error
                return View(section);
            }
        }


        [HttpPost("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            Response<Section> response = await _sectionsService.DeleteAsync(id);

            if (response.IsSuccess)
            {
                _notifyService.Success(response.Message);
                return RedirectToAction(nameof(Index));
            }

            _notifyService.Error(response.Errors.First());
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> Toggle(int Id, bool Hide)
        {
            ToggleSectionRequest request = new ToggleSectionRequest { SectionId = Id, Hide = Hide };
            Response<Section> response = await _sectionsService.ToggleSectionAsync(request);

            _notifyService.Success("Sección actualizada con éxito");

            return RedirectToAction(nameof(Index));
        }
    }
}
