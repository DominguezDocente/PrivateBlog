namespace PrivateBlog.Web.Core.Middlewares
{
    public class HttpCustomMethodOverrideMiddleware
    {
        private readonly RequestDelegate _next;

        public HttpCustomMethodOverrideMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase)
                && context.Request.HasFormContentType)
            {
                IFormCollection form = await context.Request.ReadFormAsync();
                string? method = form["_method"].FirstOrDefault(); // Obtener el primer valor si existe

                if (!string.IsNullOrEmpty(method))
                {
                    // Convertir el método a mayúsculas y establecerlo en la solicitud
                    context.Request.Method = method.ToUpperInvariant();
                }
            }

            await _next(context);
        }
    }
}