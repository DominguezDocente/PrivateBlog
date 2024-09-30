using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.Helpers;
using PrivateBlog.Web.Services;

namespace PrivateBlog.Web.Controllers
{
    public class SectionsController : Controller
    {
        private readonly ISectionsService _sectionsService;

        public SectionsController(ISectionsService sectionsService)
        {
            _sectionsService = sectionsService;
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
                    return RedirectToAction(nameof(Index));
                }

                // TODO: Mostrar mensaje de error
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
    }
}
