using PrivateBlog.Web.DTOs;

namespace PrivateBlog.Web.Core
{
    public class Response<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public T Result { get; set; }

        public static implicit operator Response<T>(Response<IEnumerable<PermissionForDTO>> v)
        {
            throw new NotImplementedException();
        }
    }
}
