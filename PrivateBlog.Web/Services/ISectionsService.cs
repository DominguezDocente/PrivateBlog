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

        public Task<Response<PaginationResponse<Section>>> GetListAsync(PaginationRequest request);

        public Task<Response<Section>> GetOneAsync(int id);

        public Task<Response<Section>> EditAsync(Section model);

        public Task<Response<Section>> DeleteAsync(int id);

        public Task<Response<Section>> ToggleSectionAsync(ToggleSectionRequest request);
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

                await _context.AddAsync(section);
                await _context.SaveChangesAsync();

                return ResponseHelper<Section>.MakeResponseSuccess(section, "Sección creada con éxito");
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
                IQueryable<Section> queryable = _context.Sections.AsQueryable();

                if (!string.IsNullOrWhiteSpace(request.Filter))
                {
                    queryable = queryable.Where(s => s.Name.ToLower().Contains(request.Filter.ToLower()));
                }

                PagedList<Section> list = await PagedList<Section>.ToPagedListAsync(queryable, request);


                PaginationResponse<Section> result = new PaginationResponse<Section> 
                {
                    List = list,
                    TotalCount = list.TotalCount,
                    RecordsPerPage = list.RecordsPerPage,
                    CurrentPage = list.CurrentPage,
                    TotalPages = list.TotalPages,

                    //Faltante:
                    Filter = request.Filter,
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
                    return ResponseHelper<Section>.MakeResponseFail($"La sección con id '{id}' no existe.");
                }

                return ResponseHelper<Section>.MakeResponseSuccess(section);
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

                return ResponseHelper<Section>.MakeResponseSuccess(model, "Sección editada con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<Section>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<Section>> DeleteAsync(int id)
        {
            try
            {
                Section? section = await _context.Sections.FirstOrDefaultAsync(s => s.Id == id);

                if (section is null)
                {
                    return ResponseHelper<Section>.MakeResponseFail($"La sección con id '{id}' no existe.");
                }

                _context.Sections.Remove(section);
                await _context.SaveChangesAsync();

                return ResponseHelper<Section>.MakeResponseSuccess("Sección eliminada con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<Section>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<Section>> ToggleSectionAsync(ToggleSectionRequest request)
        {
            try
            {
                Section? model = await _context.Sections.FindAsync(request.Id);

                if (model == null)
                {
                    return ResponseHelper<Section>.MakeResponseFail($"No existe seeción con id '{request.Id}'");
                }

                model.IsHidden = request.Hide;

                _context.Sections.Update(model);
                await _context.SaveChangesAsync();

                return ResponseHelper<Section>.MakeResponseSuccess("Sección Actualizada con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<Section>.MakeResponseFail(ex);
            }
        }
    }
}