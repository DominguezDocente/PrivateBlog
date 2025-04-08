using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Web.Models;
using Serilog;

namespace PrivateBlog.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            Log.Warning("Log de advertencia");
            Log.Error("Log de Error");
            Log.Fatal("Log Fatal");
            Log.Information("Log de Información");
            Log.Debug("Log de Debug");

            try
            {
                int a = 13;
                int b = 0;
                int r = a / b;
            }
            catch(Exception ex)
            {
                Log.Error(ex, "Ha ocurrido un error en HomeController.Index");
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
