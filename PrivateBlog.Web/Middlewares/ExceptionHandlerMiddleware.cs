using PrivateBlog.Application.Exceptions;
using PrivateBlog.Domain.Exceptions;

namespace PrivateBlog.Web.Middlewares
{
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

                string message = "Ha ocurrido un error inesperado.";

                switch (ex)
                {
                    case BusinessRuleException rule:
                        message = rule.Message;
                        break;
                    case MediatorException mediatorEx:
                        message = mediatorEx.Message;
                        break;
                    case CustomValidationException validation when validation.ValidationErrors.Count > 0:
                        message = string.Join(" ", validation.ValidationErrors);
                        break;
                    case CustomValidationException validationEx:
                        message = validationEx.Message;
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
