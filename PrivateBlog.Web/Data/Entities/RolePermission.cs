namespace PrivateBlog.Web.Data.Entities
{
    public class RolePermission
    {
        public required Guid PrivateBlogRoleId { get; set; }
        public required Guid PermissionId { get; set; }
        public PrivateBlogRole PrivateBlogRole { get; set; }
        public Permission Permission { get; set; }

    }
}
