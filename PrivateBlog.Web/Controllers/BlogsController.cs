using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Core.Pagination;
using PrivateBlog.Web.DTOs.Blog;
using PrivateBlog.Web.DTOs.Section;
using PrivateBlog.Web.Helpers.Abstractions;
using PrivateBlog.Web.Services.Abstractions;
using PrivateBlog.Web.Services.Implementations;

namespace PrivateBlog.Web.Controllers
{
    public class BlogsController : Controller
    {
        private readonly INotyfService _notyfService;
        private readonly IBlogsService _blogsService;
        private readonly ISelectListHelper _selectListHelper;

        public BlogsController(INotyfService notyfService, IBlogsService blogsService, ISelectListHelper selectListHelper)
        {
            _notyfService = notyfService;
            _blogsService = blogsService;
            _selectListHelper = selectListHelper;
        }



        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] PaginationRequest request)
        {
            Response<PaginationResponse<BlogDTO>> response = await _blogsService.GetPaginationAsync(request);

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
            CreateBlogDTO dto = new CreateBlogDTO
            {
                Sections = await _selectListHelper.GetSectionsSelectListAsync()
            };

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateBlogDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notyfService.Error("Debe ajustar los errores de validación");
                dto.Sections = await _selectListHelper.GetSectionsSelectListAsync();
                return View(dto);
            }

            Response<CreateBlogDTO> response = await _blogsService.CreateAsync(dto);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                dto.Sections = await _selectListHelper.GetSectionsSelectListAsync();
                return View(dto);
            }

            _notyfService.Success(response.Message);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] Guid id)
        {
            Response<BlogDTO> response = await _blogsService.GetOneAsync(id);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                return RedirectToAction(nameof(Index));
            }

            UpdateBlogDTO dto = new UpdateBlogDTO
            {
                Id = response.Result.Id,
                Title = response.Result.Title,
                Content = response.Result.Content,
                SectionId = response.Result.SectionId,
                Sections = await _selectListHelper.GetSectionsSelectListAsync()
            };

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] UpdateBlogDTO dto)
        {
            if (!ModelState.IsValid)
            {
                _notyfService.Error("Debe ajustar los errores de validación");
                dto.Sections = await _selectListHelper.GetSectionsSelectListAsync();
                return View(dto);
            }

            Response<BlogDTO> response = await _blogsService.UpdateAsync(dto);

            if (!response.IsSuccess)
            {
                _notyfService.Error(response.Message);
                dto.Sections = await _selectListHelper.GetSectionsSelectListAsync();
                return View(dto);
            }

            _notyfService.Success(response.Message);
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            Response response = await _blogsService.DeleteAsync(id);

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
