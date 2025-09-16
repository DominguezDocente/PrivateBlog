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
    public class SectionsService : CustomQueryableOperationsService, ISectionsService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public SectionsService(DataContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<SectionDTO>> CreateAsync(SectionDTO dto)
        {
            //try
            //{
            //    Section section = _mapper.Map<Section>(dto);

            //    Guid id = Guid.NewGuid();
            //    section.Id = id;
            //    await _context.Sections.AddAsync(section);
            //    await _context.SaveChangesAsync();

            //    dto.Id = id;
            //    return Response<SectionDTO>.Success(dto, "Sección creada con éxito");
            //}
            //catch(Exception ex)
            //{
            //    return Response<SectionDTO>.Failure(ex);
            //}

            return await CreateAsync<Section, SectionDTO>(dto);
        }

        public async Task<Response<object>> DeleteAsync(Guid id)
        {
            //try
            //{
            //    Section? section = await _context.Sections.FirstOrDefaultAsync(s => s.Id == id);

            //    if (section is null)
            //    {
            //        return Response<object>.Failure($"No existe sección con id: {id}");
            //    }

            //    _context.Sections.Remove(section);
            //    await _context.SaveChangesAsync();

            //    return Response<object>.Success("Sección eliminada con éxito");
            //}
            //catch (Exception ex)
            //{
            //    return Response<object>.Failure(ex);
            //}

            return await DeleteAsync<Section>(id);
        }

        public async Task<Response<SectionDTO>> EditAsync(SectionDTO dto)
        {
            //try
            //{
            //    Section? section = await _context.Sections.AsNoTracking()
            //                                              .FirstOrDefaultAsync(s => s.Id == dto.Id);

            //    if (section is null)
            //    {
            //        return Response<SectionDTO>.Failure($"No existe sección con id: {dto.Id}");
            //    }

            //    section = _mapper.Map<Section>(dto);
            //    _context.Sections.Update(section);
            //    await _context.SaveChangesAsync();

            //    return Response<SectionDTO>.Success(dto, "Sección actualizada con éxito");
            //}
            //catch (Exception ex)
            //{
            //    return Response<SectionDTO>.Failure(ex);
            //}


            return await EditAsync<Section, SectionDTO>(dto, dto.Id);
        }

        public async Task<Response<List<SectionDTO>>> GetListAsync()
        {
            //try
            //{
            //    List<Section> sections = await _context.Sections.ToListAsync();

            //    List<SectionDTO> list = _mapper.Map <List<SectionDTO>> (sections);

            //    return Response<List<SectionDTO>>.Success(list);
            //}
            //catch (Exception ex)
            //{
            //    return Response<List<SectionDTO>>.Failure(ex);
            //}

            return await GetCompleteListAsync<Section, SectionDTO>();
        }

        public async Task<Response<SectionDTO>> GetOneAsync(Guid id)
        {
            //try
            //{
            //    Section? section = await _context.Sections.FirstOrDefaultAsync(s => s.Id == id);

            //    if (section is null)
            //    {
            //        return Response<SectionDTO>.Failure($"No existe sección con id: {id}");
            //    }

            //    SectionDTO dto = _mapper.Map<SectionDTO>(section);

            //    return Response<SectionDTO>.Success(dto, "Sección obtenida con éxito");
            //}
            //catch (Exception ex)
            //{
            //    return Response<SectionDTO>.Failure(ex);
            //}

            return await GetOneAsync<Section, SectionDTO>(id);
        }

        public async Task<Response<object>> ToggleAsync(ToggleSectionStatusDTO dto)
        {
            try
            {
                Section? section = await _context.Sections.AsNoTracking()
                                                          .FirstOrDefaultAsync(s => s.Id == dto.SectionId);

                if (section is null)
                {
                    return Response<object>.Failure($"No existe sección con id: {dto.SectionId}");
                }

                section.IsHidden = dto.Hide;
                _context.Sections.Update(section);
                await _context.SaveChangesAsync();

                return Response<object>.Success("Sección actualizada con éxito");
            }
            catch (Exception ex)
            {
                return Response<object>.Failure(ex);
            }
        }
    }
}
