using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Application.UseCases.Account.Queries.GetAccessibleBlogById;
using PrivateBlog.Application.UseCases.Account.Queries.GetAccessibleBlogsBySection;
using PrivateBlog.Application.UseCases.Account.Queries.GetAccessibleSections;
using PrivateBlog.Application.Utilities.Mediator;
using PrivateBlog.Web.Middlewares;
using PrivateBlog.Web.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace PrivateBlog.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMediator _mediator;
        private readonly INotyfService _notifyService;

        public HomeController(IMediator mediator, INotyfService notifyService)
        {
            _mediator = mediator;
            _notifyService = notifyService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                IReadOnlyList<AccessibleSectionItemDTO> sections =
                    await _mediator.Send(new GetAccessibleSectionsQuery { UserId = userId });

                return View(sections);
            }
            catch (Exception ex)
            {
                _notifyService.Error(ex.Message);
                return View(Array.Empty<AccessibleSectionItemDTO>());
            }
        }

        [Authorize]
        public async Task<IActionResult> Section(Guid id)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                AccessibleSectionBlogsDTO section =
                    await _mediator.Send(new GetAccessibleBlogsBySectionQuery { UserId = userId, SectionId = id });

                return View(section);
            }
            catch (Exception ex)
            {
                _notifyService.Error(ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        [Authorize]
        public async Task<IActionResult> Blog(Guid id)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                AccessibleBlogDetailDTO blog =
                    await _mediator.Send(new GetAccessibleBlogByIdQuery { UserId = userId, BlogId = id });

                return View(blog);
            }
            catch (Exception ex)
            {
                _notifyService.Error(ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            string? message = HttpContext.Session.GetString(ExceptionHandlerMiddleware.ERROR_MESSAGE_SESSION_KEY);
            HttpContext.Session.Remove(ExceptionHandlerMiddleware.ERROR_MESSAGE_SESSION_KEY);

            return View(new ErrorViewModel { Message = message });
        }
    }
}
