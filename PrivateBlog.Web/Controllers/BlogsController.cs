using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Core.Pagination;
using PrivateBlog.Web.DTOs;
using PrivateBlog.Web.Helpers;
using PrivateBlog.Web.Services;

namespace PrivateBlog.Web.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogsService _blogsService;
        private readonly INotyfService _notifyService;
        private readonly ICombosHelper _combosHelper;

        public BlogsController(INotyfService notifyService, IBlogsService blogsService, ICombosHelper combosHelper)
        {
            _notifyService = notifyService;
            _blogsService = blogsService;
            _combosHelper = combosHelper;
        }


        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] PaginationRequest request)
        {
            Response<PaginationResponse<BlogDTO>> response = await _blogsService.GetPaginationAsync(request);
            return View(response.Result);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            BlogDTO dto = new BlogDTO { Sections =  await _combosHelper.GetComboSections()};
            return View(dto);
        }
    }
}
