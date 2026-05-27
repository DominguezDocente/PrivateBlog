using PrivateBlog.Application.Exceptions;
using PrivateBlog.Domain.Exceptions;
using System.Text.Json;

namespace PrivateBlog.Api.Middlewares
{
    public class ApiExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiExceptionMiddleware(RequestDelegate next)
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

                (int statusCode, object body) = MapException(ex);
                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(body));
            }
        }

        private static (int StatusCode, object Body) MapException(Exception ex)
        {
            return ex switch
            {
                CustomValidationException validation when validation.Errors.Count > 0 =>
                    (StatusCodes.Status400BadRequest, new { errors = validation.Errors }),

                CustomValidationException validation =>
                    (StatusCodes.Status400BadRequest, new { message = validation.Message }),

                BussinesRuleException rule =>
                    (StatusCodes.Status400BadRequest, new { message = rule.Message }),

                MediatorException mediator =>
                    (StatusCodes.Status400BadRequest, new { message = mediator.Message }),

                _ =>
                    (StatusCodes.Status500InternalServerError, new { message = "Ha ocurrido un error inesperado." })
            };
        }
    }

    public static class ApiExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseApiExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiExceptionMiddleware>();
        }
    }
}
