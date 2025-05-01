using PrivateBlog.Web.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace PrivateBlog.Web.DTOs
{
    public class PrivateBlogRoleDTO
    {
        public int Id { get; set; }

        [Display(Name = "Rol")]
        [MaxLength(64, ErrorMessage = "Elcampo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Name { get; set; } = null!;

        public List<PermissionForRoleDTO>? Permissions { get; set; }
        public string? PermissionIds { get; set; }

        public List<SectionForRoleDTO>? Sections { get; set; }
        public string? SectionIds { get; set; }
    }

    public class SectionForRoleDTO : SectionDTO
    {
        public bool Selected { get; set; }
    }

    public class PermissionForRoleDTO : PermissionDTO
    {
        public bool Selected { get; set; }
    }
}
