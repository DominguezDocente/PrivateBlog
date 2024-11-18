using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Core.Pagination;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.DTOs;
using PrivateBlog.Web.Models;
using PrivateBlog.Web.Services;
using System.Diagnostics;

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

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard([FromQuery] int? RecordsPerPage,
                                                   [FromQuery] int? Page,
                                                   [FromQuery] string? Filter)
        {

            PaginationRequest request = new PaginationRequest
            {
                RecordsPerPage = RecordsPerPage ?? 15,
                Page = Page ?? 1,
                Filter = Filter
            };

            Response<PaginationResponse<Section>> response = await _homeService.GetSectionsAsync(request);
            return View(response.Result);
        }

        [HttpGet]
        public async Task<IActionResult> Section([FromRoute] int id,
                                                 [FromQuery] int? RecordsPerPage,
                                                 [FromQuery] int? Page,
                                                 [FromQuery] string? Filter)
        {

            PaginationRequest request = new PaginationRequest
            {
                RecordsPerPage = RecordsPerPage ?? 15,
                Page = Page ?? 1,
                Filter = Filter
            };

            Response<SectionDTO> response = await _homeService.GetSectionAsync(request, id);
            return View(response.Result);
        }

        [HttpGet]
        public async Task<IActionResult> Blog([FromRoute] int id)
        {
            Response<Blog> response = await _homeService.GetBlogAsync(id);
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
