using Microsoft.AspNetCore.Mvc.Rendering;

namespace PrivateBlog.Web.Helpers.Abstractions
{
    public interface ICombosHelper
    {
        public Task<List<SelectListItem>> GetComboSections();
    }
}
