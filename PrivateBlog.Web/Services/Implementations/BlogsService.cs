using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Core.Pagination;
using PrivateBlog.Web.Data;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.DTOs;
using PrivateBlog.Web.Services.Abtractions;

namespace PrivateBlog.Web.Services.Implementations
{
    public class BlogsService : CustomQueryableOperationsService, IBlogsService
    {
        private readonly DataContext _context;

        public BlogsService(DataContext context, IMapper mapper) : base (context, mapper)
        {
            _context = context;   
        }

        public async Task<Response<BlogDTO>> CreateAsync(BlogDTO dto)
        {
            return await CreateAsync<Blog, BlogDTO>(dto);
        }

        public async Task<Response<object>> DeleteAsync(Guid id)
        {
            return await DeleteAsync<Blog>(id);
        }

        public async Task<Response<BlogDTO>> EditAsync(BlogDTO dto)
        {
            return await EditAsync<Blog, BlogDTO>(dto, dto.Id);
        }

        public async Task<Response<BlogDTO>> GetOneAsync(Guid id)
        {
            return await GetOneAsync<Blog, BlogDTO>(id);
        }

        public async Task<Response<PaginationResponse<BlogDTO>>> GetPaginatedListAsync(PaginationRequest request)
        {
            IQueryable<Blog> query = _context.Blogs.Include(b => b.Section)
                                                   .Select(b => new Blog 
                                                   {
                                                       Id = b.Id,
                                                       Name = b.Name,

                                                       Section = new Section 
                                                       {
                                                           Id = b.Section.Id,
                                                           Name = b.Section.Name
                                                       },

                                                       SectionId = b.SectionId                                                       
                                                   })
                                                   .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Filter))
            {
                query = query.Where(s => s.Name.ToLower().Contains(request.Filter.ToLower()));
            }

            return await GetPaginationAsync<Blog, BlogDTO>(request, query);
        }
    }
}
