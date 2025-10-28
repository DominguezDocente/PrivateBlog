using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Core.Attributes;
using PrivateBlog.Web.Core.Pagination;
using PrivateBlog.Web.DTOs;
using PrivateBlog.Web.Services.Abtractions;

namespace PrivateBlog.Web.Controllers
{
    public class SectionsController : Controller
    {
        private readonly ISectionsService _sectionsService;
        private readonly INotyfService _notyfService;

        public SectionsController(ISectionsService sectionsService, INotyfService notyfService)
        {
            _sectionsService = sectionsService;
            _notyfService = notyfService;
        }

        [HttpGet]
        [CustomAuthorize(permission: "showSections", module: "Secciones")]
        public async Task<IActionResult> Index([FromQuery] PaginationRequest request)
        {
            Response<PaginationResponse<SectionDTO>> response = await _sectionsService.GetPaginatedListAsync(request);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                return RedirectToAction("Index", "Home");
            }

            return View(response.Result);
        }

        [HttpGet]
        [CustomAuthorize("Secciones", "createSections")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [CustomAuthorize("createSections", "Secciones")]
        public async Task<IActionResult> Create([FromForm] SectionDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notyfService.Error("Debe ajustar los errores de validación");
                return View(dto);
            }

            Response<SectionDTO> response = await _sectionsService.CreateAsync(dto);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                return View(dto);
            }

            _notyfService.Success(response.Message);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [CustomAuthorize("updateSections", "Secciones")]
        public async Task<IActionResult> Edit([FromRoute] Guid id)
        {
            Response<SectionDTO> response = await _sectionsService.GetOneAsync(id);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                return RedirectToAction(nameof(Index));
            }

            return View(response.Result);
        }

        [HttpPost]
        [CustomAuthorize("updateSections", "Secciones")]
        public async Task<IActionResult> Edit([FromForm] SectionDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notyfService.Error("Debe ajustar los errores de validación");
                return View(dto);
            }

            Response<SectionDTO> response = await _sectionsService.EditAsync(dto);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                return View(dto);
            }

            _notyfService.Success(response.Message);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [CustomAuthorize("deleteSections", "Secciones")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Response<object> response = await _sectionsService.DeleteAsync(id);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
            }
            else
            {
                _notyfService.Success(response.Message);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [CustomAuthorize("updateSections", "Secciones")]
        public async Task<IActionResult> Toggle([FromForm] ToggleSectionStatusDTO dto)
        {
            Response<object> response = await _sectionsService.ToggleAsync(dto);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
            }
            else
            {
                _notyfService.Success(response.Message);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

