using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.DTOs;
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

        public async Task<IActionResult> Index()
        {
            Response<List<SectionDTO>> list = await _sectionsService.GetList();
            return View(list.Result);
        }
    }
}
