using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Core.Pagination;
using PrivateBlog.Web.DTOs.Section;
using PrivateBlog.Web.Services.Abstractions;

namespace PrivateBlog.Web.Controllers
{
    public class SectionsController : Controller
    {
        private readonly INotyfService _notyfService;
        private readonly ISectionsService _sectionsService;

        public SectionsController(INotyfService notyfService, ISectionsService sectionsService)
        {
            _notyfService = notyfService;
            _sectionsService = sectionsService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            PaginationRequest request = PaginationRequest.Default;
            Response<PaginationResponse<SectionDTO>> response = await _sectionsService.GetPaginationAsync(request);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                return RedirectToAction("Index", "Home");
            }

            return View(response.Result);
        }
    }
}
