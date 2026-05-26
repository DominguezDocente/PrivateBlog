using PrivateBlog.Application.UseCases.Roles.Queries.GetPermissionsByModule;
using PrivateBlog.Application.UseCases.Sections.Queries.GetSectionsOptions;

namespace PrivateBlog.Web.DTOs.Roles
{
    public interface IRolePermissionsForm
    {
        List<Guid> PermissionIds {  get; set; }
        IReadOnlyList<PermissionModuleGroupDTO> PermissionModules {  get; set; }
        public List<Guid> SectionIds { get; set; }
        public IReadOnlyList<SectionOptionDTO> SectionOptions { get; set; }
    }
}
