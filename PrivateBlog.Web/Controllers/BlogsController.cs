using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Core.Pagination;
using PrivateBlog.Web.DTOs;
using PrivateBlog.Web.Helpers.Abstractions;
using PrivateBlog.Web.Services.Abtractions;
using PrivateBlog.Web.Services.Implementations;
using System.Threading.Tasks;

namespace PrivateBlog.Web.Controllers
{
    public class BlogsController : Controller
    {
        private readonly INotyfService _notyfService;
        private readonly IBlogsService _blogsService;
        private readonly ICombosHelper _combosHelper;

        public BlogsController(INotyfService notyfService, IBlogsService blogsService, ICombosHelper combosHelper)
        {
            _notyfService = notyfService;
            _blogsService = blogsService;
            _combosHelper = combosHelper;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] PaginationRequest request)
        {
            Response<PaginationResponse<BlogDTO>> response = await _blogsService.GetPaginatedListAsync(request);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                return RedirectToAction("Index", "Home");
            }

            return View(response.Result);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            BlogDTO dto = new BlogDTO
            {
                Sections = await _combosHelper.GetComboSections()
            };

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BlogDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notyfService.Error("Debe ajustar los errores de validación");
                dto.Sections = await _combosHelper.GetComboSections();
                return View(dto);
            }

            Response<BlogDTO> response = await _blogsService.CreateAsync(dto);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                dto.Sections = await _combosHelper.GetComboSections();
                return View(dto);
            }

            _notyfService.Success(response.Message);
            dto.Sections = await _combosHelper.GetComboSections();
            return RedirectToAction(nameof(Index));
        }
    }
}
