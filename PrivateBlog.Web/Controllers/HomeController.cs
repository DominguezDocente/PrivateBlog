using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Core.Pagination;
using PrivateBlog.Web.DTOs;
using PrivateBlog.Web.Models;
using PrivateBlog.Web.Services;
using Serilog;

namespace PrivateBlog.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeService _homeService;

        public HomeController(ILogger<HomeController> logger, IHomeService homeService)
        {
            _logger = logger;
            _homeService = homeService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] PaginationRequest request)
        {
            Response<PaginationResponse<SectionDTO>> response = await _homeService.GetSectionsAsync(request);
            return View(response.Result);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Section([FromRoute] int id, [FromQuery] PaginationRequest request)
        {
            Response<SectionDTO> response = await _homeService.GetSectionAsync(request, id);
            return View(response.Result);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Blog([FromRoute] int id)
        {
            Response<BlogDTO> response = await _homeService.GetBlogAsync(id);
            return View(response.Result);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
