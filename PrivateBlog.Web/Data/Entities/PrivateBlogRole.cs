using System.ComponentModel.DataAnnotations;

namespace PrivateBlog.Web.Data.Entities
{
    public class PrivateBlogRole
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(32)]
        public required string Name { get; set; }

        public IEnumerable<RolePermission>? RolePermissions { get; set; }
    }
}
