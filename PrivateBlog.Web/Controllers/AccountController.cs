using Microsoft.AspNetCore.Mvc;

namespace PrivateBlog.Web.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult NotAuthorized()
        {
            return View();
        }
    }
}
