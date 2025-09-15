using AutoMapper;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Data;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.DTOs;
using PrivateBlog.Web.Services.Abtractions;

namespace PrivateBlog.Web.Services.Implementations
{
    public class SectionsService : ISectionsService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public SectionsService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<SectionDTO>> CreateAsync(SectionDTO dto)
        {
            try
            {
                Section section = _mapper.Map<Section>(dto);

                Guid id = Guid.NewGuid();
                section.Id = id;
                await _context.Sections.AddAsync(section);
                await _context.SaveChangesAsync();

                dto.Id = id;
                return Response<SectionDTO>.Success(dto, "Sección creada con éxito");
            }
            catch(Exception ex)
            {
                return Response<SectionDTO>.Failure(ex);
            }
        }

        public async Task<Response<List<SectionDTO>>> GetListAsync()
        {
            try
            {
                List<Section> sections = await _context.Sections.ToListAsync();

                List<SectionDTO> list = _mapper.Map <List<SectionDTO>> (sections);

                return Response<List<SectionDTO>>.Success(list);
            }
            catch (Exception ex)
            {
                return Response<List<SectionDTO>>.Failure(ex);
            }
        }
    }
}
