using PrivateBlog.Web.Core;

namespace PrivateBlog.Web.Helpers
{
    public static class ResponseHelper<T>
    {
        public static Response<T> MakeResponseSuccess(T model, string message = "tarea realizada con éxito")
        {
            return new Response<T>
            {
                IsSuccess = true,
                Message = message,
                Result = model,
            };
        }
        public static Response<T> MakeResponseSuccess(string message = "Tarea realizada con éxito")
        {
            return new Response<T>
            {
                IsSuccess = true,
                Message = message,
            };
        }

        public static Response<T> MakeResponseFail(Exception ex, string message = "Ha ocurrido un error al generar la solicitud.")
        {
            return new Response<T> 
            {
                IsSuccess = false,
                Message = message,
                Errors = new List<string>
                {
                    ex.Message,
                    //ex.InnerException.Message
                }
            };
        }
        public static Response<T> MakeResponseFail(string message = "Ha ocurrido un error al generar la solicitud.")
        {
            return new Response<T>
            {
                IsSuccess = false,
                Message = message,
            };
        }
    }
}
