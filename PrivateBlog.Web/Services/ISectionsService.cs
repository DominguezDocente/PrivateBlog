using Microsoft.EntityFrameworkCore;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Core.Pagination;
using PrivateBlog.Web.Data;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.Helpers;
using PrivateBlog.Web.Requests;
using System.Collections.Generic;

namespace PrivateBlog.Web.Services
{
    public interface ISectionsService
    {
        public Task<Response<Section>> CreateAsync(Section model);

        public Task<Response<Section>> DeleteteAsync(int id);

        public Task<Response<Section>> EditAsync(Section model);

        public Task<Response<PaginationResponse<Section>>> GetListAsync(PaginationRequest request);

        public Task<Response<Section>> GetOneAsync(int id);

        public Task<Response<Section>> ToggleAsync(ToggleSectionStatusRequest request);
    }

    public class SectionsService : ISectionsService
    {
        private readonly DataContext _context;

        public SectionsService(DataContext context)
        {
            _context = context;
        }

        public async Task<Response<Section>> CreateAsync(Section model)
        {
            try
            {
                Section section = new Section
                {
                    Name = model.Name,
                };

                await _context.Sections.AddAsync(section);
                await _context.SaveChangesAsync();

                return ResponseHelper<Section>.MakeResponseSuccess(section, "Sección creada con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<Section>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<Section>> DeleteteAsync(int id)
        {
            try
            {
                Response<Section> response = await GetOneAsync(id);

                if (!response.IsSuccess)
                {
                    return response;
                }

                _context.Sections.Remove(response.Result);
                await _context.SaveChangesAsync();

                return ResponseHelper<Section>.MakeResponseSuccess(null, "Sección eliminada con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<Section>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<Section>> EditAsync(Section model)
        {
            try
            {
                _context.Sections.Update(model);
                await _context.SaveChangesAsync();

                return ResponseHelper<Section>.MakeResponseSuccess(model, "Sección actualizada con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<Section>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<PaginationResponse<Section>>> GetListAsync(PaginationRequest request)
        {
            try
            {
                IQueryable<Section> query = _context.Sections.AsQueryable();

                if (!string.IsNullOrWhiteSpace(request.Filter))
                {
                    query = query.Where(s => s.Name.ToLower().Contains(request.Filter.ToLower()));
                }

                PagedList<Section> list = await PagedList<Section>.ToPagedListAsync(query, request);

                PaginationResponse<Section> result = new PaginationResponse<Section>
                {
                    List = list,
                    TotalCount = list.TotalCount,
                    RecordsPerPage = list.RecordsPerPage,
                    CurrentPage = list.CurrentPage,
                    TotalPages = list.TotalPages,
                    Filter = request.Filter
                };

                return ResponseHelper<PaginationResponse<Section>>.MakeResponseSuccess(result, "Secciones obtenidas con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<PaginationResponse<Section>>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<Section>> GetOneAsync(int id)
        {
            try
            {
                Section? section = await _context.Sections.FirstOrDefaultAsync(s => s.Id == id);

                if (section is null)
                {
                    return ResponseHelper<Section>.MakeResponseFail("La sección con el id indicado no existe");
                }

                return ResponseHelper<Section>.MakeResponseSuccess(section);
            }
            catch (Exception ex)
            {
                return ResponseHelper<Section>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<Section>> ToggleAsync(ToggleSectionStatusRequest request)
        {
            try
            {
                Response<Section> response = await GetOneAsync(request.SectionId);

                if (!response.IsSuccess)
                {
                    return response;
                }

                Section section = response.Result;

                section.IsHidden = request.Hide;
                _context.Sections.Update(section);
                await _context.SaveChangesAsync();

                return ResponseHelper<Section>.MakeResponseSuccess(null, "Sección actualizada con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<Section>.MakeResponseFail(ex);
            }
        }
    }
}