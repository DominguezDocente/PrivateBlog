using Microsoft.AspNetCore.Mvc.Rendering;

namespace PrivateBlog.Web.Helpers.Abstractions
{
    public interface ISelectListHelper
    {
        Task<List<SelectListItem>> GetSectionsSelectListAsync();
    }
}
