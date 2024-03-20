using Microsoft.EntityFrameworkCore;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Data;
using PrivateBlog.Web.Data.Entities;
using System.Collections.Generic;

namespace PrivateBlog.Web.Services
{
    public interface ISectionsService
    {
        public Task<Response<Section>> CreateSectionAsync(Section model);
        public Task<Response<List<Section>>> GetListAsync();
    }

    public class SectionsService : ISectionsService
    {
        private readonly DataContext _context;

        public SectionsService(DataContext context)
        {
            _context = context;
        }

        public async Task<Response<Section>> CreateSectionAsync(Section model)
        {
            try
            {
                Section section = new Section
                {
                    Name = model.Name,
                };

                await _context.Sections.AddAsync(section);
                await _context.SaveChangesAsync();

                return new Response<Section>
                {
                    IsSuccess = true,
                    Message = "Sección creada con éxito",
                    Result = section
                };
            }
            catch (Exception ex)
            {
                return new Response<Section>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response<List<Section>>> GetListAsync()
        {
            try
            {
                List<Section> list = await _context.Sections.ToListAsync();

                return new Response<List<Section>> 
                {
                    IsSuccess = true,
                    Message = "Secciones obtenidas con éxito",
                    Result = list
                };

            }
            catch (Exception ex)
            {
                return new Response<List<Section>>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
        }
    }
}
