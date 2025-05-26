using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Web.Core;

namespace PrivateBlog.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        public static ObjectResult ControllerBasicValidation<T>(Response<T> response, int? statusCode = null)
        {
            if (statusCode is not null)
            {
                return new ObjectResult(response)
                {
                    StatusCode = statusCode
                };
            }

            if (response.IsSuccess)
            {
                return new ObjectResult(response)
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }

            return new ObjectResult(response)
            {
                StatusCode = StatusCodes.Status400BadRequest
            };
        }
    }
}
