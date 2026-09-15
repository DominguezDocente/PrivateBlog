using AutoMapper;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Core.Pagination;
using PrivateBlog.Web.Data;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.DTOs.Section;
using PrivateBlog.Web.Services.Abstractions;

namespace PrivateBlog.Web.Services.Implementations
{
    public class SectionsService : CustomQueryableOperationsService, ISectionsService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public SectionsService(DataContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<CreateSectionDTO>> CreateAsync(CreateSectionDTO dto)
        {
            return await CreateAsync<CreateSectionDTO, Section>(dto);
        }

        public Task<Response<object>> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Response<SectionDTO>> GetOneAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Response<PaginationResponse<SectionDTO>>> GetPaginationAsync(PaginationRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<Response<object>> ToggleAsync(ToggleSectionStatusDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task<Response<SectionDTO>> UpdateAsync(UpdateSectionDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
