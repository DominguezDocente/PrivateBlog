using System.ComponentModel.DataAnnotations;

namespace PrivateBlog.Web.DTOs.Role
{
    public class RoleDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<PermissionsForRoleDTO>? Permissions { get; set; }
        public string? PermissionIds { get; set; } // Ej 548214fd-4fa6-4d8d-be96-724f1fe36d82,dcbf682d-fb2c-44bf-9969-63bb212845ba, etc

        public List<SectionsForRoleDTO>? Sections { get; set; }
        public string? SectionIds { get; set; }
    }
}
