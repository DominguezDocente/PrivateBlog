using PrivateBlog.Web.Core;
using PrivateBlog.Web.DTOs;

namespace PrivateBlog.Web.Services.Abtractions
{
    public interface ISectionsService
    {
        public Task<Response<SectionDTO>> CreateAsync(SectionDTO dto);
        public Task<Response<List<SectionDTO>>> GetListAsync();
    }
}
