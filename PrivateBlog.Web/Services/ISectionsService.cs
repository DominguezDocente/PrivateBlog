using AutoMapper;
using Humanizer;
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
        public Task<Response<SectionDTO>> CreateAsync(SectionDTO dto);
        public Task<Response<object>> DeleteAsync(int id);
        public Task<Response<SectionDTO>> EditAsync(SectionDTO dto);
        public Task<Response<List<SectionDTO>>> GetListAsync();
        public Task<Response<SectionDTO>> GetOneAsync(int id);
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

        public async Task<Response<SectionDTO>> CreateAsync(SectionDTO dto)
        {
            try
            {
                Section section = _mapper.Map<Section>(dto);

                await _context.AddAsync(section);
                await _context.SaveChangesAsync();

                return ResponseHelper<SectionDTO>.MakeResponseSuccess(dto, "Sección creada con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<SectionDTO>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<object>> DeleteAsync(int id)
        {
            try
            {
                // TODO: ajustar con AutoMapper
                Section? section = await _context.Sections.FirstOrDefaultAsync(s => s.Id == id);

                if (section is null)
                {
                    return ResponseHelper<object>.MakeResponseFail($"No existe sección con id {id}");
                }

                _context.Sections.Remove(section);
                await _context.SaveChangesAsync();

                return ResponseHelper<object>.MakeResponseSuccess("Sección eliminada con éxito");

            }
            catch (Exception ex)
            {
                return ResponseHelper<object>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<SectionDTO>> EditAsync(SectionDTO dto)
        {
            try
            {
                Section? section = await _context.Sections.FirstOrDefaultAsync(s => s.Id == dto.Id);

                if (section is null)
                {
                    return ResponseHelper<SectionDTO>.MakeResponseFail($"No existe sección con id {dto.Id}");
                }

                // TODO: Ajustar
                //section = _mapper.Map<Section>(dto);
                section.Name = dto.Name;
                _context.Update(section);
                await _context.SaveChangesAsync();

                return ResponseHelper<SectionDTO>.MakeResponseSuccess(dto, "Sección actualizada con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<SectionDTO>.MakeResponseFail(ex);
            }
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

        public async Task<Response<SectionDTO>> GetOneAsync(int id)
        {
            try
            {
                Section? section = await _context.Sections.FirstOrDefaultAsync(s => s.Id == id);

                if (section is null)
                {
                    return ResponseHelper<SectionDTO>.MakeResponseFail($"No existe sección con id {id}");
                }

                SectionDTO dto = _mapper.Map<SectionDTO>(section);

                //_context.Entry(section).State = EntityState.Unchanged;

                return ResponseHelper<SectionDTO>.MakeResponseSuccess(dto, "Sección obtenida con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<SectionDTO>.MakeResponseFail(ex);
            }
        }
    }
}
