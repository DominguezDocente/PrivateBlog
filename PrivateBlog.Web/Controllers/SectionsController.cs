using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Data.Entities;
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

        public async Task<IActionResult> Index()
        {
            _notify.Success("Secciones");

            Response<List<Section>> response = await _sectionsService.GetListAsync();

            return View(response.Result);
        }
    }
}
