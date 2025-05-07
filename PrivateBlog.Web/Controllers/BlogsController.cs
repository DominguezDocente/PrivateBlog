using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Web.Core.Attributes;
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
        [CustomAuthorize(permission: "showBLogs", module: "Blogs")]
        public async Task<IActionResult> Index([FromQuery] PaginationRequest request)
        {
            Response<PaginationResponse<BlogDTO>> response = await _blogsService.GetPaginationAsync(request);
            return View(response.Result);
        }

        [HttpGet]
        [CustomAuthorize(permission: "createBLogs", module: "Blogs")]
        public async Task<IActionResult> Create()
        {
            BlogDTO dto = new BlogDTO { Sections =  await _combosHelper.GetComboSections()};
            return View(dto);
        }

        [HttpPost]
        [CustomAuthorize(permission: "createBLogs", module: "Blogs")]
        public async Task<IActionResult> Create(BlogDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notifyService.Error("Debe ajustar los errores de validación");
                dto.Sections = await _combosHelper.GetComboSections();
                return View(dto);
            }

            Response<BlogDTO> response = await _blogsService.CreateAsync(dto);

            if (!response.IsSuccess)
            {
                _notifyService.Error(response.Message);
                dto.Sections = await _combosHelper.GetComboSections();
                return View(dto);
            }

            _notifyService.Success(response.Message);
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        [CustomAuthorize(permission: "updateBlogs", module: "Blogs")]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            Response<BlogDTO> response = await _blogsService.GetOneAsync(id);

            if (!response.IsSuccess)
            {
                _notifyService.Error(response.Message);
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
                _notifyService.Error("Debe ajustar los errores de validación");
                dto.Sections = await _combosHelper.GetComboSections();
                return View(dto);
            }

            Response<BlogDTO> response = await _blogsService.EditAsync(dto);

            if (!response.IsSuccess)
            {
                _notifyService.Error(response.Message);
                dto.Sections = await _combosHelper.GetComboSections();
                return View(dto);
            }

            _notifyService.Success(response.Message);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [CustomAuthorize(permission: "deleteBlogs", module: "Blogs")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            Response<object> response = await _blogsService.DeleteAsync(id);

            if (!response.IsSuccess)
            {
                _notifyService.Error(response.Message);
            }
            else
            {
                _notifyService.Success(response.Message);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
