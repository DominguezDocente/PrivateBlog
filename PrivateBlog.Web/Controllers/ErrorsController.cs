using Microsoft.AspNetCore.Mvc;

namespace PrivateBlog.Web.Controllers
{
    public class ErrorsController : Controller
    {
        [Route("Errors/{statusCode}")]
        public IActionResult Error(int statusCode)
        {
            string errorMesage = "Ha ocurrido un error.";

            switch (statusCode)
            {
                case StatusCodes.Status404NotFound: 
                    errorMesage = "La página que estás untentando acceder no existe"; 
                    break;

                case StatusCodes.Status403Forbidden:
                    errorMesage = "No tienes permiso para estar aquí";
                    break;

                case StatusCodes.Status401Unauthorized:
                    errorMesage = "Debes iniciar sesión";
                    break;
            }

            ViewBag.ErrorMessage = errorMesage;

            return View(statusCode);
        }
    }
}
