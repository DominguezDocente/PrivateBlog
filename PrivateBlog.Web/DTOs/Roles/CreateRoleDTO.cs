using System.ComponentModel.DataAnnotations;
using PrivateBlog.Application.UseCases.Roles.Queries.GetPermissionsByModule;

namespace PrivateBlog.Web.DTOs.Roles
{
    public sealed class CreateRoleDTO : IRolePermissionsForm
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, MinimumLength = 2)]
        [Display(Name = "Nombre del rol")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Permisos")]
        public List<Guid> PermissionIds { get; set; } = [];

        public IReadOnlyList<PermissionModuleGroupDTO> PermissionModules { get; set; } = [];
    }
}
