using PrivateBlog.Application.Exceptions;
using PrivateBlog.Domain.Exceptions;

namespace PrivateBlog.Web.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        public const string ERROR_MESSAGE_SESSION_KEY = "ErrorMessage";

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
            catch(Exception ex) 
            {
                string message = "Ha ocurrido un error";

                switch (ex)
                {
                    case BussinesRuleException rule:

                        message = rule.Message;
                        break;

                    case MediatorException mediatorEx:

                        message = mediatorEx.Message;
                        break;

                    case CustomValidationException validationEx when validationEx.Errors.Count > 0:

                        message = string.Join(" ", validationEx.Errors);
                        break;

                    case CustomValidationException validationEx:

                        message = validationEx.Message;
                        break;
                }

                await context.Session.LoadAsync(context.RequestAborted);
                context.Session.SetString(ERROR_MESSAGE_SESSION_KEY, message);

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
