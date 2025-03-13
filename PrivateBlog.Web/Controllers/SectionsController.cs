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

        public SectionsController(ISectionsService sectionsService)
        {
            _sectionsService = sectionsService;
        }

        public async Task<IActionResult> Index()
        {
            Response<List<SectionDTO>> response = await _sectionsService.GetListAsync();
            return View(response.Result);
        }
    }
}
