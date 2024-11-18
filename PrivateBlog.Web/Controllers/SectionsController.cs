using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Core.Attributes;
using PrivateBlog.Web.Core.Pagination;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.Helpers;
using PrivateBlog.Web.Requests;
using PrivateBlog.Web.Services;

namespace PrivateBlog.Web.Controllers
{
    [Authorize]
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
        [CustomAuthorize(permission: "showSections", module: "Secciones")]
        public async Task<IActionResult> Index([FromQuery] int? RecordsPerPage,
                                               [FromQuery] int? Page,
                                               [FromQuery] string? Filter)
        {
            PaginationRequest request = new PaginationRequest
            {
                RecordsPerPage = RecordsPerPage ?? 15,
                Page = Page ?? 1,
                Filter = Filter
            };

            Response<PaginationResponse<Section>> response = await _sectionsService.GetListAsync(request);
            return View(response.Result);
        }

        [HttpGet]
        [CustomAuthorize(permission: "createSections", module: "Secciones")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [CustomAuthorize(permission: "showSections", module: "Secciones")]
        public async Task<IActionResult> Create(Section section)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _notifyService.Error("Debe ajustar los errores de validación");
                    return View(section);
                }

                Response<Section> response = await _sectionsService.CreateAsync(section);

                if (response.IsSuccess)
                {
                    _notifyService.Success(response.Message);
                    return RedirectToAction(nameof(Index));
                }

                _notifyService.Error(response.Message);
                return View(response);
            }
            catch (Exception ex)
            {
                return View(section);
            }
        }

        [HttpGet]
        [CustomAuthorize(permission: "updateSections", module: "Secciones")]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            Response<Section> response = await _sectionsService.GetOneAsync(id);

            if (response.IsSuccess)
            {
                return View(response.Result);
            }

            _notifyService.Error(response.Message);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [CustomAuthorize(permission: "updateSections", module: "Secciones")]
        public async Task<IActionResult> Edit(Section section)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _notifyService.Error("Debe ajustar los errores de validación");
                    return View(section);
                }

                Response<Section> response = await _sectionsService.EditAsync(section);

                if (response.IsSuccess)
                {
                    _notifyService.Success(response.Message);
                    return RedirectToAction(nameof(Index));
                }

                _notifyService.Error(response.Message);
                return View(response);
            }
            catch (Exception ex)
            {
                _notifyService.Error(ex.Message);
                return View(section);
            }
        }

        [HttpPost]
        [CustomAuthorize(permission: "deleteSections", module: "Secciones")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            Response<Section> response = await _sectionsService.DeleteteAsync(id);

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

        [HttpPost]
        [CustomAuthorize(permission: "updateSections", module: "Secciones")]
        public async Task<IActionResult> Toggle(int SectionId, bool Hide)
        {
            ToggleSectionStatusRequest request = new ToggleSectionStatusRequest
            {
                Hide = Hide,
                SectionId = SectionId
            };

            Response<Section> response = await _sectionsService.ToggleAsync(request);

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