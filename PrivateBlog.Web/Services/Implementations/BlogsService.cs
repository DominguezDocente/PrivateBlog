using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Core.Pagination;
using PrivateBlog.Web.Data;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.DTOs.Blog;
using PrivateBlog.Web.DTOs.Section;
using PrivateBlog.Web.Services.Abstractions;

namespace PrivateBlog.Web.Services.Implementations
{
    public class BlogsService : CustomQueryableOperationsService, IBlogsService
    {
        private readonly DataContext _context;

        public BlogsService(DataContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
        }

        public async Task<Response<CreateBlogDTO>> CreateAsync(CreateBlogDTO dto)
        {
            return await CreateAsync<CreateBlogDTO, Blog>(dto);
        }

        public async Task<Response> DeleteAsync(Guid id)
        {
            return await DeleteAsync<Blog>(id);
        }

        public async Task<Response<BlogDTO>> GetOneAsync(Guid id)
        {
            return await GetOneAsync<BlogDTO, Blog>(id);
        }

        public async Task<Response<PaginationResponse<BlogDTO>>> GetPaginationAsync(PaginationRequest request)
        {
            IQueryable<Blog> query = _context.Blogs.Include(b => b.Section)
                                                   .Select(b => new Blog
                                                   {
                                                       Id = b.Id,
                                                       Title = b.Title,

                                                       Section = new Section
                                                       {
                                                           Id =  b.Section.Id,
                                                           Name = b.Section.Name
                                                       }
                                                   })
                                                   .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Filter))
            {
                query = query.Where(s => s.Title.ToLower().Contains(request.Filter.ToLower()));
            }

            return await GetPagedListAsync<BlogDTO, Blog>(request, query);
        }

        public async Task<Response<BlogDTO>> UpdateAsync(UpdateBlogDTO dto)
        {
            Response<UpdateBlogDTO> result = await UpdateAsync<UpdateBlogDTO, Blog>(dto, dto.Id);

            BlogDTO dtoResponse = new BlogDTO
            {
                Id = result.Result.Id,
                Title = result.Result.Title,
                Content = result.Result.Content,
                Section = result.Result.Section,
                SectionId = result.Result.SectionId
            };

            return Response<BlogDTO>.Success(dtoResponse, result.Message);
        }
    }
}
