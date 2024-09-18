using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Web.Data.Entities;

namespace PrivateBlog.Web.Controllers
{
    public class SectionsController : Controller
    {
        public IActionResult Index()
        {
            List<Section> sections = new List<Section> 
            {
                new Section
                {
                    Id = 1,
                    Name = "General",
                },
                
                new Section
                {
                    Id = 2,
                    Name = "Telecomunicaciones",
                },
                
                new Section
                {
                    Id = 3,
                    Name = "Hacking",
                },
            };

            return View(sections);
        }
    }
}
