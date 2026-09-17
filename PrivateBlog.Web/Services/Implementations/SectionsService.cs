using AutoMapper;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Response<object>> DeleteAsync(Guid id)
        {
           return await DeleteAsync<Section>(id);
        }

        public async Task<Response<SectionDTO>> GetOneAsync(Guid id)
        {
            return await GetOneAsync<SectionDTO, Section>(id);
        }

        public async Task<Response<PaginationResponse<SectionDTO>>> GetPaginationAsync(PaginationRequest request)
        {
            return await GetPagedListAsync<SectionDTO, Section>(request);
        }

        public async Task<Response<object>> ToggleAsync(ToggleSectionStatusDTO dto)
        {
            try
            {
                Section? section = await _context.Sections.FirstOrDefaultAsync(s => s.Id == dto.Id);

                if (section is null)
                {
                    return Response<object>.Failure($"No existe sección con id: {dto.Id}");
                }

                section.IsHidden = dto.Hide;
                _context.Sections.Update(section);
                await _context.SaveChangesAsync();

                return Response<object>.Success("Sección actualizada con éxito");
            }
            catch(Exception ex)
            {
                return Response<object>.Failure(ex);
            }
        }

        public async Task<Response<SectionDTO>> UpdateAsync(SectionDTO dto)
        {
            return await UpdateAsync<SectionDTO, Section>(dto, dto.Id);
        }
    }
}
