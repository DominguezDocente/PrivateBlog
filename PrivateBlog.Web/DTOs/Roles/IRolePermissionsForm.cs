using PrivateBlog.Application.UseCases.Roles.Queries.GetPermissionsByModule;

namespace PrivateBlog.Web.DTOs.Roles
{
    public interface IRolePermissionsForm
    {
        List<Guid> PermissionIds {  get; set; }
        IReadOnlyList<PermissionModuleGroupDTO> PermissionModules {  get; set; }
    }
}
