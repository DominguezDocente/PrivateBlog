using PrivateBlog.Web.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace PrivateBlog.Web.DTOs
{
    public class PrivateBlogRoleDTO
    {
        public int Id { get; set; }

        [Display(Name = "Rol")]
        [MaxLength(64, ErrorMessage = "El campo {0} debe terner máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Name { get; set; } = null!;

        public List<PermissionForDTO>? Permissions { get; set; }

        public string? PermissionIds { get; set; }

        public List<SectionForDTO>? Sections { get; set; }

        public string? SectionIds { get; set; }
    }

    public class SectionForDTO : Section
    {
        public bool Selected { get; set; } = false;
    }

    public class PermissionForDTO : Permission
    {
        public bool Selected { get; set; }
    }
}
