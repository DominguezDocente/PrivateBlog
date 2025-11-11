using Microsoft.AspNetCore.Mvc.Rendering;

namespace PrivateBlog.Web.Helpers.Abstractions
{
    public interface ICombosHelper
    {
        Task<IEnumerable<SelectListItem>> GetComboRoles();
        public Task<List<SelectListItem>> GetComboSections();
    }
}
