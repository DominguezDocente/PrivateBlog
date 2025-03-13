using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Data;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.DTOs;
using PrivateBlog.Web.Helpers;

namespace PrivateBlog.Web.Services
{
    public interface ISectionsService
    {
        public Task<Response<List<SectionDTO>>> GetListAsync();
    }

    public class SectionsService : ISectionsService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public SectionsService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<List<SectionDTO>>> GetListAsync()
        {
            try
            {
                List<Section> sections = await _context.Sections.ToListAsync();

                List<SectionDTO> list = _mapper.Map<List<SectionDTO>>(sections);

                return ResponseHelper<List<SectionDTO>>.MakeResponseSuccess(list);
            }
            catch(Exception ex)
            {
                return ResponseHelper<List<SectionDTO>>.MakeResponseFail(ex);
            }
        }
    }
}
