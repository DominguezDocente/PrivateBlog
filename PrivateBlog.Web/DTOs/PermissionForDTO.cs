using PrivateBlog.Web.Data.Entities;

namespace PrivateBlog.Web.DTOs
{
    public class PermissionForDTO : Permission
    {
        public bool Selected { get; set; } = false;
    }
}
