using AspNetCoreHero.ToastNotification.Abstractions;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.Requests;
using PrivateBlog.Web.Services;

namespace PrivateBlog.Web.Controllers
{
    public class SectionsController : Controller
    {
        private readonly ISectionsService _sectionsService;
        private readonly INotyfService _notify;

        public SectionsController(ISectionsService sectionsService, INotyfService notify)
        {
            _sectionsService = sectionsService;
            _notify = notify;
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
        public async Task<IActionResult> Create(Section model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _notify.Error("Debe ajustar los errores de validación.");
                    return View(model);
                }

                Response<Section> response = await _sectionsService.CreateAsync(model);

                if (response.IsSuccess)
                {
                    _notify.Success(response.Message);
                    return RedirectToAction(nameof(Index));
                }

                _notify.Error(response.Message);
                return View(model);
            }
            catch (Exception ex)
            {
                _notify.Error(ex.Message);
                return View(model);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            Response<Section> response = await _sectionsService.GetOneAsync(id);

            if (response.IsSuccess)
            {
                return View(response.Result);
            }

            _notify.Error(response.Errors.First());
            return RedirectToAction(nameof(Index));
        }

        [HttpPut]
        public async Task<IActionResult> Update(Section model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _notify.Error("Debe ajustar los errores de validación.");
                    return View(model);
                }

                Response<Section> response = await _sectionsService.EditAsync(model);

                if (response.IsSuccess)
                {
                    _notify.Success(response.Message);
                    return RedirectToAction(nameof(Index));
                }

                _notify.Error(response.Errors.First());
                return View(model);
            }
            catch (Exception ex)
            {
                _notify.Error(ex.Message);
                return View(model);
            }
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            Response<Section> response = await _sectionsService.DeleteAsync(id);

            if (response.IsSuccess)
            {
                _notify.Success(response.Message);
                return RedirectToAction(nameof(Index));
            }

            _notify.Error(response.Errors.First());
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [HttpPut]
        public async Task<IActionResult> Toggle(int Id, bool Hide)
        {
            ToggleSectionRequest request = new ToggleSectionRequest { Id = Id, Hide = Hide };
            Response<Section> response = await _sectionsService.ToggleSectionAsync(request);

            _notify.Success("Sección actualizada con éxito");

            return RedirectToAction(nameof(Index));
        }
    }
}