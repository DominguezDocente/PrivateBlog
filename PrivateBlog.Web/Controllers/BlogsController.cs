using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Core.Attributes;
using PrivateBlog.Web.Core.Pagination;
using PrivateBlog.Web.DTOs;
using PrivateBlog.Web.Helpers.Abstractions;
using PrivateBlog.Web.Services.Abtractions;
using PrivateBlog.Web.Services.Implementations;
using System.Threading.Tasks;

namespace PrivateBlog.Web.Controllers
{
    [Authorize]
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
        [CustomAuthorize(permission: "showBlogs", module: "Blogs")]
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

        [HttpGet]
        [CustomAuthorize(permission: "updateBlogs", module: "Blogs")]
        public async Task<IActionResult> Edit([FromRoute] Guid id)
        {
            Response<BlogDTO> response = await _blogsService.GetOneAsync(id);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                return RedirectToAction(nameof(Index));
            }

            response.Result.Sections = await _combosHelper.GetComboSections();
            return View(response.Result);
        }

        [HttpPost]
        [CustomAuthorize(permission: "updateBlogs", module: "Blogs")]
        public async Task<IActionResult> Edit(BlogDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notyfService.Error("Debe ajustar los errores de validación");
                dto.Sections = await _combosHelper.GetComboSections();
                return View(dto);
            }

            Response<BlogDTO> response = await _blogsService.EditAsync(dto);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                dto.Sections = await _combosHelper.GetComboSections();
                return View(dto);
            }

            _notyfService.Success(response.Message);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [CustomAuthorize(permission: "deleteBlogs", module: "Blogs")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Response<object> response = await _blogsService.DeleteAsync(id);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
            }
            else
            {
                _notyfService.Success(response.Message);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
