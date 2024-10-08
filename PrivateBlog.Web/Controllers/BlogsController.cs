using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.DTOs;
using PrivateBlog.Web.Helpers;
using PrivateBlog.Web.Services;

namespace PrivateBlog.Web.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogsService _blogsService;
        private readonly ICombosHelper _combosHelper;
        private readonly INotyfService _notifyService;
        private readonly IConverterHelper _converterHelper;

        public BlogsController(IBlogsService blogService, ICombosHelper combosHelper, INotyfService notifyService, IConverterHelper converterHelper)
        {
            _blogsService = blogService;
            _combosHelper = combosHelper;
            _notifyService = notifyService;
            _converterHelper = converterHelper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            Response<List<Blog>> response = await _blogsService.GetListAsync();
            return View(response.Result);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            BlogDTO dto = new BlogDTO 
            {
                Sections = await _combosHelper.GetComboSections(),
            };

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BlogDTO dto)
        {
            if (!ModelState.IsValid) 
            {
                _notifyService.Error("Debe ajustar los errores de validación");
                dto.Sections = await _combosHelper.GetComboSections();
                return View(dto);
            }

            Response<Blog> response = await _blogsService.CreateAsync(dto);

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
        public async Task<IActionResult> Edit([FromRoute] int id)
        {

            Response<Blog> response = await _blogsService.GetOneAsync(id);

            if (response.IsSuccess)
            {
                BlogDTO dto = await _converterHelper.ToBlogDTO(response.Result);

                return View(dto);
            }

            _notifyService.Error(response.Message);
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> Edit(BlogDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _notifyService.Error("Debe ajustar los errores de validación");
                    return View(dto);
                }

                Response<Blog> response = await _blogsService.EditAsync(dto);

                if (response.IsSuccess)
                {
                    _notifyService.Success(response.Message);
                    return RedirectToAction(nameof(Index));
                }

                _notifyService.Error(response.Message);
                dto.Sections = await _combosHelper.GetComboSections();
                return View(dto);
            }
            catch (Exception ex)
            {
                _notifyService.Error(ex.Message);
                dto.Sections = await _combosHelper.GetComboSections();
                return View(dto);
            }
        }
    }
}
