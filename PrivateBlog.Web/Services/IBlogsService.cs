using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Core.Pagination;
using PrivateBlog.Web.Data;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.DTOs;
using PrivateBlog.Web.Helpers;

namespace PrivateBlog.Web.Services
{
    public interface IBlogsService
    {
        public Task<Response<BlogDTO>> CreateAsync(BlogDTO dto);
        public Task<Response<object>> DeleteAsync(int id);
        public Task<Response<BlogDTO>> EditAsync(BlogDTO dto);
        public Task<Response<BlogDTO>> GetOneAsync(int id);
        public Task<Response<PaginationResponse<BlogDTO>>> GetPaginationAsync(PaginationRequest request);
    }

    public class BlogsService : CustomQueryableOperations, IBlogsService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public BlogsService(DataContext context, IMapper mapper) : base (context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<BlogDTO>> CreateAsync(BlogDTO dto)
        {
            return await CreateAsync<Blog, BlogDTO>(dto);
        }

        public async Task<Response<object>> DeleteAsync(int id)
        {
            Response<object> response = await DeleteAsync<Blog>(id);
            response.Message = !response.IsSuccess ? $"El blog con id: {id} no existe" : response.Message;
            return response;
        }

        public async Task<Response<BlogDTO>> EditAsync(BlogDTO dto)
        {
            return await EditAsync<Blog, BlogDTO>(dto, dto.Id);
        }

        public async Task<Response<BlogDTO>> GetOneAsync(int id)
        {
            return await GetOneAsync<Blog, BlogDTO>(id);
        }

        public async Task<Response<PaginationResponse<BlogDTO>>> GetPaginationAsync(PaginationRequest request)
        {
            IQueryable<Blog> query = _context.Blogs.Include(b => b.Section)
                                                   .Select(b => new Blog
                                                   {
                                                       Id = b.Id,
                                                       Name = b.Name,
                                                       Section = b.Section
                                                   })
                                                   .AsQueryable();

            if (!string.IsNullOrEmpty(request.Filter))
            {
                query = query.Where(b => b.Name.ToLower()
                                               .Contains(request.Filter
                                               .ToLower()));
            }

            return await GetPaginationAsync<Blog, BlogDTO>(request, query);
        }
    }
}
