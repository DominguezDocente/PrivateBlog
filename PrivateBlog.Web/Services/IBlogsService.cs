using Azure.Core;
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

        public Task<Response<Blog>> EditAsync(BlogDTO dto);

        public Task<Response<PaginationResponse<Blog>>> GetListAsync(PaginationRequest request);

        public Task<Response<Blog>> GetOneAsync(int id);
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

        public async Task<Response<Blog>> CreateAsync(BlogDTO dto)
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

        public async Task<Response<Blog>> EditAsync(BlogDTO dto)
        {
            try
            {
                Blog? blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == dto.Id);

                if (blog is null)
                {
                    return ResponseHelper<Blog>.MakeResponseFail($"No existe Blog con id '{dto.Id}'");
                }

                //blog = _converterHelper.ToBlog(dto);

                blog.Title = dto.Title;
                blog.Content = dto.Content;
                blog.SectionId = dto.SectionId;

                _context.Blogs.Update(blog);
                await _context.SaveChangesAsync();

                return ResponseHelper<Blog>.MakeResponseSuccess(blog, "Blog actualizado con éxito");
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
                IQueryable<Blog> query = _context.Blogs.AsQueryable().Include(b => b.Section);

                if (!string.IsNullOrWhiteSpace(request.Filter))
                {
                    query = query.Where(s => s.Title.ToLower().Contains(request.Filter.ToLower()));
                }

                PagedList<Blog> list = await PagedList<Blog>.ToPagedListAsync(query, request);

                PaginationResponse<Blog> result = new PaginationResponse<Blog>
                {
                    List = list,
                    TotalCount = list.TotalCount,
                    RecordsPerPage = list.RecordsPerPage,
                    CurrentPage = list.CurrentPage,
                    TotalPages = list.TotalPages,
                    Filter = request.Filter
                };

                return ResponseHelper<PaginationResponse<Blog>>.MakeResponseSuccess(result, "Blogs obtenidas con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<PaginationResponse<Blog>>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<Blog>> GetOneAsync(int id)
        {
            try
            {
                Blog? blog = await _context.Blogs.FirstOrDefaultAsync(s => s.Id == id);

                if (blog is null)
                {
                    return ResponseHelper<Blog>.MakeResponseFail("El blog con el id indicado no existe");
                }

                return ResponseHelper<Blog>.MakeResponseSuccess(blog);
            }
            catch (Exception ex)
            {
                return ResponseHelper<Blog>.MakeResponseFail(ex);
            }
        }
    }
}