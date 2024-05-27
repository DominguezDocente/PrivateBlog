using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Web.Core.Attributes;
using PrivateBlog.Web.Core.Pagination;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.Services;
using PrivateBlog.Web.DTOs;
using PrivateBlog.Web.Helpers;
using PrivateBlog.Web.Requests;

namespace PrivateBlog.Web.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogsService _blogService;
        private readonly ICombosHelper _combosHelper;
        private readonly INotyfService _noty;

        public BlogsController(IBlogsService blogService, INotyfService noty, ICombosHelper combosHelper)
        {
            _blogService = blogService;
            _noty = noty;
            _combosHelper = combosHelper;
        }

        [HttpGet]
        [CustomAuthorize(permission: "showBlogs", module: "Blogs")]
        public async Task<IActionResult> Index([FromQuery] int? RecordsPerPage,
                                               [FromQuery] int? Page,
                                               [FromQuery] string? Filter)
        {
            PaginationRequest paginationRequest = new PaginationRequest
            {
                RecordsPerPage = RecordsPerPage ?? 15,
                Page = Page ?? 1,
                Filter = Filter,
            };

            Response<PaginationResponse<Blog>> response = await _blogService.GetListAsync(paginationRequest);

            return View(response.Result);
        }

        [HttpGet]
        [CustomAuthorize(permission: "createBlogs", module: "Blogs")]
        public async Task<IActionResult> Create()
        {
            BlogDTO dto = new BlogDTO
            {
                Sections = await _combosHelper.GetComboSections()
            };

            return View(dto);
        }

        [HttpPost]
        [CustomAuthorize(permission: "createBlogs", module: "Blogs")]
        public async Task<IActionResult> Create(BlogDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _noty.Error("Debe ajustar los errores de validación");
                dto.Sections = await _combosHelper.GetComboSections();
                return View(dto);
            }

            Response<Blog> response = await _blogService.CreateAsync(dto);

            if (!response.IsSuccess)
            {
                _noty.Error(response.Message);
                dto.Sections = await _combosHelper.GetComboSections();
                return View(dto);
            }

            _noty.Success("Blog creado con éxito");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [CustomAuthorize(permission: "updateBlogs", module: "Blogs")]
        public async Task<IActionResult> Toggle(int Id, bool Hide)
        {
            ToggleSectionRequest request = new ToggleSectionRequest { Id = Id, Hide = Hide };
            Response<Section> response = await _blogService.ToggleBlogAsync(request);

            if (!response.IsSuccess)
            {
                _noty.Error(response.Message);
            }
            else
            {
                _noty.Success(response.Message);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
