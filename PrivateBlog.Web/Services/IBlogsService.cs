using Microsoft.EntityFrameworkCore;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Data;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.DTOs;
using PrivateBlog.Web.Helpers;

namespace PrivateBlog.Web.Services
{
    public interface IBlogsService
    {
        public Task<Response<Blog>> CreateAsync(BlogDTO dto);
        public Task<Response<List<Blog>>> GetListAsync();
    }

    public class BlogsService : IBlogsService
    {
        private readonly DataContext _context;
        private readonly IConverterHelper _converterHelper;

        public BlogsService(DataContext context, IConverterHelper converterHelper)
        {
            _context = context;
            _converterHelper = converterHelper;
        }

        public async  Task<Response<Blog>> CreateAsync(BlogDTO dto)
        {
            try
            {
                Blog blog = _converterHelper.ToBlog(dto);

                await _context.Blogs.AddAsync(blog);
                await _context.SaveChangesAsync();

                return ResponseHelper<Blog>.MakeResponseSuccess(blog, "Blog creado con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<Blog>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<List<Blog>>> GetListAsync()
        {
            try
            {
                List<Blog> list = await _context.Blogs.Include(b => b.Section)
                                                      .ToListAsync();

                return ResponseHelper<List<Blog>>.MakeResponseSuccess(list);
            }
            catch (Exception ex) 
            {
                return ResponseHelper<List<Blog>>.MakeResponseFail(ex);
            }
        }
    }
}
