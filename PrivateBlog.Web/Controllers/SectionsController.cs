using AspNetCoreHero.ToastNotification.Abstractions;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.DTOs;
using PrivateBlog.Web.Services;
using System.Threading.Tasks;

namespace PrivateBlog.Web.Controllers
{
    public class SectionsController : Controller
    {
        private readonly ISectionsService _sectionsService;
        private readonly INotyfService _notifyService;

        public SectionsController(ISectionsService sectionsService, INotyfService notifyService)
        {
            _sectionsService = sectionsService;
            _notifyService = notifyService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            Response<List<SectionDTO>> response = await _sectionsService.GetListAsync();
            return View(response.Result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SectionDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notifyService.Error("Debe ajustar los errores de validación");
                return View(dto);
            }

            Response<SectionDTO> response = await _sectionsService.CreateAsync(dto);

            if (response.IsSuccess)
            {
                _notifyService.Success(response.Message);
                return RedirectToAction(nameof(Index));
            }

            _notifyService.Error(response.Message);
            return View(dto);
        }
        
        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            Response<SectionDTO> response = await _sectionsService.GetOneAsync(id);

            if (response.IsSuccess)
            {
                return View(response.Result);
            }

            _notifyService.Error(response.Message);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SectionDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notifyService.Error("Debe ajustar los errores de validación");
                return View(dto);
            }

            Response<SectionDTO> response = await _sectionsService.EditAsync(dto);

            if (response.IsSuccess)
            {
                _notifyService.Success(response.Message);
                return RedirectToAction(nameof(Index));
            }

            _notifyService.Error(response.Message);
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            Response<object> response = await _sectionsService.DeleteAsync(id);

            if (response.IsSuccess)
            {
                _notifyService.Success(response.Message);
            }
            else
            {
                _notifyService.Error(response.Message);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
