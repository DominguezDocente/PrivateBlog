using PrivateBlog.Web.Core;
using PrivateBlog.Web.Core.Pagination;
using PrivateBlog.Web.DTOs;

namespace PrivateBlog.Web.Services.Abtractions
{
    public interface ISectionsService
    {
        public Task<Response<SectionDTO>> CreateAsync(SectionDTO dto);
        public Task<Response<object>> DeleteAsync(Guid id);
        public Task<Response<SectionDTO>> EditAsync(SectionDTO dto);
        public Task<Response<List<SectionDTO>>> GetListAsync();
        public Task<Response<SectionDTO>> GetOneAsync(Guid id);
        public Task<Response<PaginationResponse<SectionDTO>>> GetPaginatedListAsync(PaginationRequest request);
        public Task<Response<object>> ToggleAsync(ToggleSectionStatusDTO dto);
    }
}
