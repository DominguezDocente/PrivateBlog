using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PrivateBlog.Web.Services.Abstractions;
using System.Security.Claims;

namespace PrivateBlog.Web.Core.Attributes
{
    public class ApiAuthorizeAttribute : TypeFilterAttribute
    {
        public ApiAuthorizeAttribute(string permissionCode) : base(typeof(ApiAuthorizeFilter))
        {
            Arguments = [permissionCode];
        }
    }

    public class ApiAuthorizeFilter : IAsyncAuthorizationFilter
    {
        private readonly string _permissionCode;
        private readonly IAccountService _accountService;

        public ApiAuthorizeFilter(string permissionCode, IAccountService accountService)
        {
            _permissionCode = permissionCode;
            _accountService = accountService;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            ClaimsPrincipal user = context.HttpContext.User;

            if (user?.Identity is null || !user.Identity.IsAuthenticated)
            {
                Response response = Response.Failure("No autenticado", ["Token no válido"]);

                context.Result = new JsonResult(response)
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
            }

            bool isAuthorized = await _accountService.AccountUserIsAuthorizedAsync(_permissionCode);

            if (!isAuthorized)
            {
                Response response = Response.Failure("Acceso denegado", ["No tienes permisoso para realizar esta acción"]);

                context.Result = new JsonResult(response)
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
            }
        }
    }
}
