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
        public Task<Response<Blog>> CreateAsync(BlogDTO dto);
        public Task<Response<PaginationResponse<Blog>>> GetListAsync(PaginationRequest request);
    }

    public class BlogsService : IBlogsService
    {
        private readonly DataContext _context;
        private readonly IUsersService _usersService;
        private readonly IConverterHelper _converterHelper;

        public BlogsService(DataContext context, IUsersService usersService, IConverterHelper converterHelper)
        {
            _context = context;
            _usersService = usersService;
            _converterHelper = converterHelper;
        }

        public async Task<Response<Blog>> CreateAsync(BlogDTO dto)
        {
            try
            {
                User? user = await _usersService.GetCurrentUserAsync();

                if (user is null) 
                {
                    return ResponseHelper<Blog>.MakeResponseFail("No hay usuario en sesión.");
                }

                Blog blog = _converterHelper.ToBlog(dto);
                blog.AuthorId = user.Id;

                await _context.Blogs.AddAsync(blog);
                await _context.SaveChangesAsync();

                return ResponseHelper<Blog>.MakeResponseSuccess(blog, "Blog creado con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<Blog>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<PaginationResponse<Blog>>> GetListAsync(PaginationRequest request)
        {
            try
            {
                IQueryable<Blog> queryable = _context.Blogs.Include(b => b.Section)
                                                           .Include(b => b.Author)
                                                           .AsQueryable();

                if (!string.IsNullOrWhiteSpace(request.Filter))
                {
                    queryable = queryable.Where(b => b.Title.ToLower().Contains(request.Filter.ToLower()));
                }

                queryable = queryable.Select(b => new Blog
                {
                    Id = b.Id,
                    Title = b.Title,
                    IsPublished = b.IsPublished,
                    Author = b.Author,
                    AuthorId = b.AuthorId,
                    Section = b.Section,
                    SectionId = b.SectionId,
                });

                PagedList<Blog> list = await PagedList<Blog>.ToPagedListAsync(queryable, request);

                PaginationResponse<Blog> result = new PaginationResponse<Blog>
                {
                    List = list,
                    TotalCount = list.TotalCount,
                    RecordsPerPage = list.RecordsPerPage,
                    CurrentPage = list.CurrentPage,
                    TotalPages = list.TotalPages,
                    Filter = request.Filter,
                };

                return ResponseHelper<PaginationResponse<Blog>>.MakeResponseSuccess(result);
            }
            catch (Exception ex) 
            {
                return ResponseHelper<PaginationResponse<Blog>>.MakeResponseFail(ex);
            }
        }
    }
}
