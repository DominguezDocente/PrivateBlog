using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrivateBlog.Web.DTOs
{
    public class PrivateBlogRoleDTO
    {
        public Guid Id { get; set; }

        [Display(Name = "Rol")]
        [MaxLength(64, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Name { get; set; }

        public List<PermissionsForRoleDTO>? Permissions { get; set; }

        public string? PermissionIds { get; set; }

        public List<SectionsForRoleDTO>? Sections { get; set; }

        public string? SectionIds { get; set; }
    }

    public class PermissionsForRoleDTO : PermissionDTO
    {
        public bool Selected { get; set; }
    }

    public class SectionsForRoleDTO : SectionDTO
    {
        public bool Selected { get; set; }
    }
}
