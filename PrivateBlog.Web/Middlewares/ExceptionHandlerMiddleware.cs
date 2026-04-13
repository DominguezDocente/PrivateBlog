using PrivateBlog.Application.Exceptions;
using PrivateBlog.Domain.Exceptions;

namespace PrivateBlog.Web.Middlewares
{
    /// <summary>
    /// Guarda el mensaje en sesión (no en TempData): TempData solo se “confirma” al final
    /// de una acción MVC, así que desde middleware el cookie de TempData no llegaba a guardarse.
    /// </summary>
    public class ExceptionHandlerMiddleware
    {
        public const string ErrorMessageSessionKey = "ErrorMessage";

        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                if (context.Response.HasStarted)
                {
                    throw;
                }

                string message;
                switch (ex)
                {
                    case BusinessRuleException businessRule:
                        message = businessRule.Message;
                        break;
                    case MediatorException mediator:
                        message = mediator.Message;
                        break;
                    case CustomValidationException validation when validation.ValidationErrors.Count > 0:
                        message = string.Join(" ", validation.ValidationErrors);
                        break;
                    case CustomValidationException validationException:
                        message = validationException.Message;
                        break;
                    default:
                        message = "Ha ocurrido un error inesperado.";
                        break;
                }

                await context.Session.LoadAsync(context.RequestAborted);
                context.Session.SetString(ErrorMessageSessionKey, message);

                context.Response.Redirect("/Home/Error");
            }
        }
    }

    public static class ExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
