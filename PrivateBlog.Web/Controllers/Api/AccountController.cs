using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using PrivateBlog.Web.DTOs.Account;
using PrivateBlog.Web.Services.Abstractions;

namespace PrivateBlog.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ApiController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            (bool isValid, List<string> errors) = ValidateModelState(ModelState);

            if (!isValid)
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity, Core.Response.Failure("Debe ajustar los errores de validación", errors));
            }

            return ControllerBasicValidation<UserTokenDTO>(await _accountService.LoginApiAsync(dto));
        }
    }
}
