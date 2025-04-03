using AutoMapper;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Core.Pagination;
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
        public Task<Response<PaginationResponse<SectionDTO>>> GetPaginationAsync(PaginationRequest request);
        public Task<Response<object>> ToggleAsync(ToggleSectionStatusDTO dto);
    }

    public class SectionsService : CustomQueryableOperations, ISectionsService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public SectionsService(DataContext context, IMapper mapper) : base (context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<SectionDTO>> CreateAsync(SectionDTO dto)
        {
            //try
            //{
            //    Section section = _mapper.Map<Section>(dto);

            //    await _context.AddAsync(section);
            //    await _context.SaveChangesAsync();

            //    return ResponseHelper<SectionDTO>.MakeResponseSuccess(dto, "Sección creada con éxito");
            //}
            //catch (Exception ex)
            //{
            //    return ResponseHelper<SectionDTO>.MakeResponseFail(ex);
            //}

            return await CreateAsync<Section, SectionDTO>(dto);
        }

        public async Task<Response<object>> DeleteAsync(int id)
        {
            //try
            //{
            //    Section? section = await _context.Sections.FirstOrDefaultAsync(s => s.Id == id);

            //    if (section is null)
            //    {
            //        return ResponseHelper<object>.MakeResponseFail($"No existe sección con id {id}");
            //    }

            //    _context.Sections.Remove(section);
            //    await _context.SaveChangesAsync();

            //    return ResponseHelper<object>.MakeResponseSuccess("Sección eliminada con éxito");

            //}
            //catch (Exception ex)
            //{
            //    return ResponseHelper<object>.MakeResponseFail(ex);
            //}

            Response<object> response = await DeleteAsync<Section>(id);
            response.Message = !response.IsSuccess ? $"La sección con id: {id} no existe" : response.Message;
            return response;
        }

        public async Task<Response<SectionDTO>> EditAsync(SectionDTO dto)
        {
            //try
            //{
            //    Section? section = await _context.Sections.AsNoTracking()
            //                                              .FirstOrDefaultAsync(s => s.Id == dto.Id);

            //    if (section is null)
            //    {
            //        return ResponseHelper<SectionDTO>.MakeResponseFail($"No existe sección con id {dto.Id}");
            //    }

            //    section = _mapper.Map<Section>(dto);
            //    _context.Update(section);
            //    await _context.SaveChangesAsync();

            //    return ResponseHelper<SectionDTO>.MakeResponseSuccess(dto, "Sección actualizada con éxito");
            //}
            //catch (Exception ex)
            //{
            //    return ResponseHelper<SectionDTO>.MakeResponseFail(ex);
            //}

            return await EditAsync<Section, SectionDTO>(dto, dto.Id);
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
            //try
            //{
            //    Section? section = await _context.Sections.FirstOrDefaultAsync(s => s.Id == id);

            //    if (section is null)
            //    {
            //        return ResponseHelper<SectionDTO>.MakeResponseFail($"No existe sección con id {id}");
            //    }

            //    SectionDTO dto = _mapper.Map<SectionDTO>(section);

            //    //_context.Entry(section).State = EntityState.Unchanged;

            //    return ResponseHelper<SectionDTO>.MakeResponseSuccess(dto, "Sección obtenida con éxito");
            //}
            //catch (Exception ex)
            //{
            //    return ResponseHelper<SectionDTO>.MakeResponseFail(ex);
            //}

            return await GetOneAsync<Section, SectionDTO>(id);
        }

        public async Task<Response<PaginationResponse<SectionDTO>>> GetPaginationAsync(PaginationRequest request)
        {
            //try
            //{
            //    IQueryable<Section> query = _context.Sections.AsQueryable();

            //    PagedList<Section> list = await PagedList<Section>.ToPagedListAsync(query, request);

            //    PaginationResponse<SectionDTO> response = new PaginationResponse<SectionDTO>
            //    {
            //        List = _mapper.Map<PagedList<SectionDTO>>(list),
            //        TotalCount = list.TotalCount,
            //        RecordsPerPage = list.RecordsPerPage,
            //        CurrentPage = list.CurrentPage,
            //        TotalPages = list.TotalPages,
            //    };

            //    return ResponseHelper<PaginationResponse<SectionDTO>>.MakeResponseSuccess(response);
            //}
            //catch (Exception ex)
            //{
            //    return ResponseHelper<PaginationResponse<SectionDTO>>.MakeResponseFail(ex);
            //}


            IQueryable<Section> query = _context.Sections.AsQueryable();

            if (!string.IsNullOrEmpty(request.Filter))
            {
                query = query.Where(b => b.Name.ToLower().Contains(request.Filter.ToLower())
                                      || b.Description.ToLower().Contains(request.Filter.ToLower()));
            }

            return await GetPaginationAsync<Section, SectionDTO>(request, query);

        }

        public async Task<Response<object>> ToggleAsync(ToggleSectionStatusDTO dto)
        {
            try
            {
                Section? section = await _context.Sections.FirstOrDefaultAsync(s => s.Id == dto.SectionId);

                if (section is null)
                {
                    return ResponseHelper<object>.MakeResponseFail($"No existe sección con id {dto.SectionId}");
                }

                section.IsHidden = dto.Hide;
                _context.Sections.Update(section);
                await _context.SaveChangesAsync();

                return ResponseHelper<object>.MakeResponseSuccess("Sección actualizada con éxito");
            }
            catch(Exception ex)
            {
                return ResponseHelper<object>.MakeResponseFail(ex);
            }
        }

    }
}
