using PrivateBlog.Web.Core;
using PrivateBlog.Web.Core.Pagination;
using PrivateBlog.Web.DTOs.Section;

namespace PrivateBlog.Web.Services.Abstractions
{
    public interface ISectionsService
    {
        public Task<Response<CreateSectionDTO>> CreateAsync(CreateSectionDTO dto);
        public Task<Response<object>> DeleteAsync(Guid id);
        public Task<Response<SectionDTO>> GetOneAsync(Guid id);
        public Task<Response<PaginationResponse<SectionDTO>>> GetPaginationAsync(PaginationRequest request);
        public Task<Response<SectionDTO>> UpdateAsync(UpdateSectionDTO dto);
        public Task<Response<object>> ToggleAsync(ToggleSectionStatusDTO dto);
    }
}
