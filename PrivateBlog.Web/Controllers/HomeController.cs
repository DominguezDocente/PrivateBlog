using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Core.Attributes;
using PrivateBlog.Web.Core.Pagination;
using PrivateBlog.Web.DTOs;
using PrivateBlog.Web.Models;
using PrivateBlog.Web.Services.Abtractions;
using PrivateBlog.Web.Services.Implementations;
using System.Diagnostics;

namespace PrivateBlog.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeService _homeService;

        public HomeController(IHomeService homeService)
        {
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
        public async Task<IActionResult> Section([FromRoute] Guid id, [FromQuery] PaginationRequest request)
        {
            Response<SectionDTO> response = await _homeService.GetSectionAsync(id, request);
            return View(response.Result);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Blog([FromRoute] Guid id)
        {
            Response<BlogDTO> response = await _homeService.GetBlogAsync(id);
            return View(response.Result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
