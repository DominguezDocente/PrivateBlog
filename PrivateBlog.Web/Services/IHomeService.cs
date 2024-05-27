using PrivateBlog.Web.Core.Pagination;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.Data;
using PrivateBlog.Web.Helpers;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ClaimsUser = System.Security.Claims.ClaimsPrincipal;
using PrivateBlog.Web.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace PrivateBlog.Web.Services
{
    public interface IHomeService
    {
        public Task<Response<Blog>> GetBlogAsync(int id);

        public Task<Response<SectionDTO>> GetSectionAsync(PaginationRequest request, int id);

        public Task<Response<PaginationResponse<Section>>> GetSectionsAsync(PaginationRequest request);
    }

    public class HomeService : IHomeService
    {
        private readonly DataContext _context;
        private IHttpContextAccessor _httpContextAccessor;
        private IUsersService _usersService;

        public HomeService(DataContext context, IHttpContextAccessor httpContextAccessor, IUsersService usersService)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _usersService = usersService;
        }

        public async Task<Response<Blog>> GetBlogAsync(int id)
        {
            try
            {
                Blog? blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id);

                if (blog is null)
                {
                    return ResponseHelper<Blog>.MakeResponseFail($"Blog con id '{id}' no existe.");
                }

                return ResponseHelper<Blog>.MakeResponseSuccess(blog);
            }
            catch (Exception ex) 
            {
                return ResponseHelper<Blog>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<SectionDTO>> GetSectionAsync(PaginationRequest request, int id)
        {
            try
            {
                Section? section = await _context.Sections.Include(s => s.RoleSections)
                                                          .Where(s => !s.IsHidden && s.Id == id)
                                                          .FirstOrDefaultAsync();

                if (section is null)
                {
                    return ResponseHelper<SectionDTO>.MakeResponseFail($"Sección con id '{id}' no existe.");
                }

                ClaimsUser? claimUser = _httpContextAccessor.HttpContext?.User;
                string? userName = claimUser.Identity.Name;
                User user = await _usersService.GetUserAsync(userName);

                bool isAuthorized = true;
                if (!await _usersService.CurrentUserIsSuperAdminAsync())
                {
                    isAuthorized = section.RoleSections.Any(rs => rs.RoleId == user.PrivateBlogRoleId);
                }

                if (!isAuthorized)
                {
                    return ResponseHelper<SectionDTO>.MakeResponseFail("No tiene autorización para consultar al Sección.");
                }

                IQueryable<Blog> queryable = _context.Blogs.Where(a => a.Section == section && a.IsPublished);

                if (!string.IsNullOrWhiteSpace(request.Filter))
                {
                    queryable = queryable.Where(q => q.Title.ToLower().Contains(request.Filter.ToLower()));
                }

                queryable = queryable.Select(a => new Blog { Title = a.Title, Id = a.Id });

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

                SectionDTO dto = new SectionDTO
                {
                    Id = section.Id,
                    IsHidden = section.IsHidden,
                    Name = section.Name,
                    PaginatedBlogs = result,
                };

                return ResponseHelper<SectionDTO>.MakeResponseSuccess(dto);
            }
            catch (Exception ex)
            {
                return ResponseHelper<SectionDTO>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<PaginationResponse<Section>>> GetSectionsAsync(PaginationRequest request)
        {
            try
            {
                ClaimsUser? claimUser = _httpContextAccessor.HttpContext?.User;
                string? userName = claimUser.Identity.Name;
                User user = await _usersService.GetUserAsync(userName);

                IQueryable<Section> query = _context.Sections.Where(s => !s.IsHidden);

                if (!await _usersService.CurrentUserIsSuperAdminAsync())
                {
                    query = query.Where(s => s.RoleSections.Any(rs => rs.RoleId == user.PrivateBlogRoleId));
                }

                if (!string.IsNullOrWhiteSpace(request.Filter))
                {
                    query = query.Where(q => q.Name.ToLower().Contains(request.Filter.ToLower()));
                }

                query = query.Select(s => new Section
                {
                    Name = s.Name,
                    Id = s.Id,
                });

                PagedList<Section> list = await PagedList<Section>.ToPagedListAsync(query, request);

                PaginationResponse<Section> result = new PaginationResponse<Section>
                {
                    List = list,
                    TotalCount = list.TotalCount,
                    RecordsPerPage = list.RecordsPerPage,
                    CurrentPage = list.CurrentPage,
                    TotalPages = list.TotalPages,
                    Filter = request.Filter,
                };

                return ResponseHelper<PaginationResponse<Section>>.MakeResponseSuccess(result);
            }
            catch (Exception ex)
            {
                return ResponseHelper<PaginationResponse<Section>>.MakeResponseFail(ex);
            }
        }
    }
}