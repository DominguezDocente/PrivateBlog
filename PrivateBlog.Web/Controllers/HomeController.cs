using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Web.Middlewares;
using PrivateBlog.Web.Models;

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
            string? message = HttpContext.Session.GetString(ExceptionHandlerMiddleware.ErrorMessageSessionKey);
            HttpContext.Session.Remove(ExceptionHandlerMiddleware.ErrorMessageSessionKey);

            return View(new ErrorViewModel { Message = message });
        }
    }
}
