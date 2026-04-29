using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PrivateBlog.Application.UseCases.Account.Queries.UserHasPermission;
using PrivateBlog.Application.Utils.Mediator;

namespace PrivateBlog.Web.Authorization
{
    /// <summary>
    /// Autorización por código de permiso (tabla <c>Permissions.Code</c>).
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class RequirePermissionAttribute : TypeFilterAttribute
    {
        public RequirePermissionAttribute(string permissionCode)
            : base(typeof(RequirePermissionFilter))
        {
            Arguments = new object[] { permissionCode };
        }
    }

    public sealed class RequirePermissionFilter : IAsyncAuthorizationFilter
    {
        private readonly IMediator _mediator;
        private readonly string _permission;

        public RequirePermissionFilter(IMediator mediator, string permissionCode)
        {
            _mediator = mediator;
            _permission = permissionCode;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            HttpContext httpContext = context.HttpContext;

            if (httpContext.User?.Identity?.IsAuthenticated != true)
            {
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                        ["controller"] = "Account",
                        ["action"] = "Login",
                        ["returnUrl"] = httpContext.Request.Path + httpContext.Request.QueryString,
                    });

                return;
            }

            string? userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                context.Result = new ForbidResult();
                return;
            }

            bool allowed = await _mediator.Send(new UserHasPermissionQuery
            {
                UserId = userId,
                PermissionCode = _permission,
            });

            if (!allowed)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
