﻿using Microsoft.EntityFrameworkCore;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Core.Pagination;
using PrivateBlog.Web.Data;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.DTOs;
using PrivateBlog.Web.Helpers;
using ClaimsUser = System.Security.Claims.ClaimsPrincipal;

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
        private readonly IHttpContextAccessor _httpContextAccesor;
        private readonly IUsersService _usersService;

        public HomeService(DataContext context, IHttpContextAccessor httpContextAccesor, IUsersService usersService)
        {
            _context = context;
            _httpContextAccesor = httpContextAccesor;
            _usersService = usersService;
        }

        public async Task<Response<Blog>> GetBlogAsync(int id)
        {
            try
            {
                Blog? blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id);

                if (blog is null)
                {
                    return ResponseHelper<Blog>.MakeResponseFail($"El blog con id '{id}' no existe.");
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
                    return ResponseHelper<SectionDTO>.MakeResponseFail($"La sección con id '{id}' no existe.");
                }

                ClaimsUser? claimsuser = _httpContextAccesor.HttpContext?.User;
                string? userName = claimsuser.Identity.Name;
                User user = await _usersService.GetUserAsync(userName);

                bool isAuthorized = true;
                if (!await _usersService.CurrentUserIsSuperAdmin())
                {
                    isAuthorized = section.RoleSections.Any(rs => rs.RoleId == user.PrivateBlogRoleId);
                }

                if (!isAuthorized)
                {
                    return ResponseHelper<SectionDTO>.MakeResponseFail("No tiene autorización para consultar esta sección");
                }

                IQueryable<Blog> query = _context.Blogs.Where(b => b.SectionId == section.Id);

                if (!string.IsNullOrWhiteSpace(request.Filter))
                {
                    query = query.Where(s => s.Title.ToLower().Contains(request.Filter.ToLower()));
                }

                query = query.Select(b => new Blog
                {
                    Id = b.Id,
                    Title = b.Title,
                });

                PagedList<Blog> list = await PagedList<Blog>.ToPagedListAsync(query, request);

                PaginationResponse<Blog> paginatedBlogsResponse = new PaginationResponse<Blog>
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
                    Name = section.Name,
                    PaginatedBlogs = paginatedBlogsResponse
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
                ClaimsUser? claimsuser = _httpContextAccesor.HttpContext?.User;
                string? userName = claimsuser.Identity.Name;
                User user = await _usersService.GetUserAsync(userName);

                IQueryable<Section> query = _context.Sections.Include(s => s.RoleSections)
                                                             .Where(s => !s.IsHidden);

                if (!await _usersService.CurrentUserIsSuperAdmin())
                {
                    query = query.Where(s => s.RoleSections.Any(rs => rs.RoleId == user.PrivateBlogRoleId));
                }

                if (!string.IsNullOrWhiteSpace(request.Filter)) 
                {
                    query = query.Where(s => s.Name.ToLower().Contains(request.Filter.ToLower()));
                }

                PagedList<Section> list = await PagedList<Section>.ToPagedListAsync(query, request);

                PaginationResponse<Section> response = new PaginationResponse<Section>
                {
                    List = list,
                    TotalCount = list.TotalCount,
                    RecordsPerPage = list.RecordsPerPage,
                    CurrentPage = list.CurrentPage,
                    TotalPages = list.TotalPages,
                    Filter = request.Filter,
                };

                return ResponseHelper<PaginationResponse<Section>>.MakeResponseSuccess(response);

            }
            catch (Exception ex) 
            { 
                return ResponseHelper<PaginationResponse<Section>>.MakeResponseFail(ex);
            }

        }
    }
}
