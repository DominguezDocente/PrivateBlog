using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Core.Pagination;
using PrivateBlog.Web.DTOs.Blog;
using PrivateBlog.Web.DTOs.Section;
using PrivateBlog.Web.Models;
using PrivateBlog.Web.Services.Abstractions;
using PrivateBlog.Web.Services.Implementations;
using System.Diagnostics;

namespace PrivateBlog.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly INotyfService _notyfService;
        private readonly IHomeService _homeService;

        public HomeController(INotyfService notyf, IHomeService homeService)
        {
            _notyfService = notyf;
            _homeService = homeService;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] PaginationRequest request)
        {
            Response<PaginationResponse<SectionDTO>> response = await _homeService.GetSectionsAsync(request);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                // TODO: Return to custom error page
                return RedirectToAction(nameof(Error));
            }

            return View(response.Result);
        }

        [HttpGet]
        public async Task<IActionResult> Section([FromRoute] Guid id, [FromQuery] PaginationRequest request)
        {
            Response<SectionDTO> response = await _homeService.GetSectionAsync(id, request);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                // TODO: Return to custom error page
                return RedirectToAction(nameof(Error));
            }

            return View(response.Result);
        }

        [HttpGet]
        public async Task<IActionResult> Blog([FromRoute] Guid id)
        {
            Response<BlogDTO> response = await _homeService.GetBlogAsync(id);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                // TODO: Return to custom error page
                return RedirectToAction(nameof(Error));
            }

            return View(response.Result);
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
