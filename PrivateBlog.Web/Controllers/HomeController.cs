using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Web.Middlewares;
using PrivateBlog.Web.Models;
using System.Diagnostics;

namespace PrivateBlog.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
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
