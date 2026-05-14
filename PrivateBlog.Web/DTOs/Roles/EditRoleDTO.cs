using PrivateBlog.Application.UseCases.Roles.Queries.GetPermissionsByModule;
using System.ComponentModel.DataAnnotations;

namespace PrivateBlog.Web.DTOs.Roles
{
    public class EditRoleDTO : IRolePermissionsForm
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, MinimumLength = 2)]
        [Display(Name = "Nombre del rol")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Permisos")]
        public List<Guid> PermissionIds { get; set; } = [];

        public IReadOnlyList<PermissionModuleGroupDTO> PermissionModules { get; set; } = [];
    }
}
