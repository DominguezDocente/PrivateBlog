using Microsoft.EntityFrameworkCore;
using PrivateBlog.Web.Core;
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

        public Task<Response<List<Section>>> GetListAsync();

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

        public async Task<Response<List<Section>>> GetListAsync()
        {
            try
            {
                List<Section> list = await _context.Sections.ToListAsync();

                return ResponseHelper<List<Section>>.MakeResponseSuccess(list, "Secciones obtenidas con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<List<Section>>.MakeResponseFail(ex);
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