using Microsoft.EntityFrameworkCore;

namespace PrivateBlog.Web.Data.Entities
{
    public class RolePermission
    {
        public int RoleId { get; set; }
        public PrivateBlogRole Role { get; set; }

        public int PermissionId { get; set; }
        public Permission Permission { get; set; }
    }
}
