using PrivateBlog.Web.Data.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace PrivateBlog.Web.Data.Entities
{
    public class PrivateBlogRole : IId
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(32)]
        [Required]
        public required string Name { get; set; }

        public ICollection<RolePermission>? RolePermissions { get; set; }
    }
}
