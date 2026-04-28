using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Application.Contracts.Pagination;
using PrivateBlog.Application.UseCases.Blogs.Commands.CreateBlog;
using PrivateBlog.Application.UseCases.Blogs.Commands.DeleteBlog;
using PrivateBlog.Application.UseCases.Blogs.Commands.UpdateBlog;
using PrivateBlog.Application.UseCases.Blogs.Queries.GetBlogById;
using PrivateBlog.Application.UseCases.Blogs.Queries.GetBlogsList;
using PrivateBlog.Application.UseCases.Sections.Queries.GetSectionsOptions;
using PrivateBlog.Application.Utilities.Mediator;
using PrivateBlog.Web.DTOs.Blogs;

namespace PrivateBlog.Web.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly INotyfService _notifyService;

        public BlogsController(IMediator mediator, INotyfService notifyService)
        {
            _mediator = mediator;
            _notifyService = notifyService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            int page = 1,
            int pageSize = PaginationRequest.DEFAULT_PAGE_SIZE,
            string? name = null,
            Guid? sectionId = null,
            bool? isPublished = null)
        {
            try
            {
                IReadOnlyList<SectionOptionDTO> sectionOptions = await _mediator.Send(new GetSectionOptionsQuery());

                PaginationResponse<BlogListItemDTO> result = await _mediator.Send(new GetBlogsListQuery
                {
                    Pagination = new PaginationRequest(page, pageSize),
                    NameFilter = name,
                    SectionIdFilter = sectionId,
                    IsPublishedFilter = isPublished,
                });

                return View(new BlogsIndexViewModel
                {
                    List = result,
                    FilterName = name ?? string.Empty,
                    FilterSectionId = sectionId,
                    FilterIsPublished = isPublished,
                    SectionOptions = sectionOptions,
                });
            }
            catch (Exception ex)
            {
                _notifyService.Error($"Error al cargar los blogs: {ex.Message}");
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            IReadOnlyList<SectionOptionDTO> sectionOptions =
                await _mediator.Send(new GetSectionOptionsQuery());

            return View(new BlogCreateViewModel { SectionOptions = sectionOptions });
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind(Prefix = "Blog")] CreateBlogDTO dto)
        {
            IReadOnlyList<SectionOptionDTO> sectionOptions =
                await _mediator.Send(new GetSectionOptionsQuery());

            if (!ModelState.IsValid)
            {
                _notifyService.Error("Debe corregir los errores de validación.");
                return View(new BlogCreateViewModel { Blog = dto, SectionOptions = sectionOptions });
            }

            try
            {
                CreateBlogCommand command = new CreateBlogCommand
                {
                    Name = dto.Name,
                    Content = dto.Content,
                    SectionId = dto.SectionId,
                    IsPublished = dto.IsPublished,
                };

                await _mediator.Send(command);
                _notifyService.Success("Blog creado con éxito.");
            }
            catch (Exception ex)
            {
                _notifyService.Error(ex.Message);
                return View(new BlogCreateViewModel { Blog = dto, SectionOptions = sectionOptions });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                BlogDetailDTO? blog = await _mediator.Send(new GetBlogByIdQuery { Id = id });

                if (blog is null)
                {
                    _notifyService.Error("No se encontró el blog.");
                    return RedirectToAction(nameof(Index));
                }

                IReadOnlyList<SectionOptionDTO> sectionOptions =
                    await _mediator.Send(new GetSectionOptionsQuery());

                EditBlogDTO editDto = new EditBlogDTO
                {
                    Id = blog.Id,
                    Name = blog.Name,
                    Content = blog.Content,
                    SectionId = blog.SectionId,
                    IsPublished = blog.IsPublished,
                };

                return View(new BlogEditViewModel { Blog = editDto, SectionOptions = sectionOptions });
            }
            catch (Exception ex)
            {
                _notifyService.Error(ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit([Bind(Prefix = "Blog")] EditBlogDTO dto)
        {
            IReadOnlyList<SectionOptionDTO> sectionOptions = await _mediator.Send(new GetSectionOptionsQuery());

            if (!ModelState.IsValid)
            {
                _notifyService.Error("Debe corregir los errores de validación.");
                return View(new BlogEditViewModel { Blog = dto, SectionOptions = sectionOptions });
            }

            try
            {
                UpdateBlogCommand command = new UpdateBlogCommand
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    Content = dto.Content,
                    SectionId = dto.SectionId,
                    IsPublished = dto.IsPublished,
                };

                await _mediator.Send(command);
                _notifyService.Success("Blog actualizado con éxito.");
            }
            catch (Exception ex)
            {
                _notifyService.Error(ex.Message);
                return View(new BlogEditViewModel { Blog = dto, SectionOptions = sectionOptions });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _mediator.Send(new DeleteBlogCommand { Id = id });
                _notifyService.Success("Blog eliminado con éxito.");
            }
            catch (Exception ex)
            {
                _notifyService.Error(ex.Message);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
