using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Core.Pagination;
using PrivateBlog.Web.Data;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.DTOs;
using PrivateBlog.Web.Services.Abtractions;
using ClaimsUser = System.Security.Claims.ClaimsPrincipal;

namespace PrivateBlog.Web.Services.Implementations
{
    public class HomeService : CustomQueryableOperationsService, IHomeService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUsersService _usersService;
        private readonly IMapper _mapper;

        public HomeService(DataContext context, 
                           IHttpContextAccessor httpContextAccessor,
                           IUsersService usersService,
                           IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _usersService = usersService;
            _mapper = mapper;
        }

        public async Task<Response<BlogDTO>> GetBlogAsync(Guid id)
        {
            return await GetOneAsync<Blog, BlogDTO>(id);
        }

        public async Task<Response<SectionDTO>> GetSectionAsync(Guid id, PaginationRequest request)
        {
            try
            {
                Section? section = await _context.Sections.Include(s => s.RoleSections)
                                                          .Where(s => !s.IsHidden && s.Id == id)
                                                          .FirstOrDefaultAsync();

                if (section is null)
                {
                    return Response<SectionDTO>.Failure($"La sección con id '{id}' no existe.");
                }

                ClaimsUser? claimsUser = _httpContextAccessor.HttpContext?.User;
                string? userName = claimsUser.Identity.Name;
                User user = await _usersService.GetUserByEmailAsync(userName);

                bool isAuthorized = true;
                if (!await _usersService.CurrentUserIsSuperAdminAsync())
                {
                    isAuthorized = section.RoleSections.Any(rs => rs.PrivateBlogRoleId == user.PrivateBlogRoleId);
                }

                if (!isAuthorized)
                {
                    return Response<SectionDTO>.Failure("No tiene autoriszación para consultar esta sección");
                }

                IQueryable<Blog> query = _context.Blogs.Where(b => b.SectionId == section.Id);
                query = query.Select(b => new Blog 
                {
                    Id = b.Id,
                    Name = b.Name,
                    SectionId = b.SectionId
                });

                if (!string.IsNullOrWhiteSpace(request.Filter))
                {
                    query = query.Where(b => b.Name.ToLower().Contains(request.Filter.ToLower()));
                }

                Response<PaginationResponse<BlogDTO>> paginationResponse = await GetPaginationAsync<Blog, BlogDTO>(request, query);
                if (!paginationResponse.IsSuccess)
                {
                    return Response<SectionDTO>.Failure(paginationResponse.Message);
                }

                SectionDTO dto = new SectionDTO
                {
                    Id = section.Id,
                    Name = section.Name,
                    Description =  section.Description,
                    PaginatedBlogs = paginationResponse.Result
                };

                return Response<SectionDTO>.Success(dto);
            }
            catch (Exception ex)
            {
                return Response<SectionDTO>.Failure(ex);
            }
        }

        public async Task<Response<PaginationResponse<SectionDTO>>> GetSectionsAsync(PaginationRequest request)
        {

            ClaimsUser? claimsUser = _httpContextAccessor.HttpContext?.User;
            string? userName = claimsUser.Identity.Name;
            User user = await _usersService.GetUserByEmailAsync(userName);

            IQueryable<Section> query = _context.Sections.Include(s => s.RoleSections)
                                                         .Where(s => !s.IsHidden);

            if (!await _usersService.CurrentUserIsSuperAdminAsync())
            {
                query = query.Where(s => s.RoleSections.Any(rs => rs.PrivateBlogRoleId == user.PrivateBlogRoleId));
            }


            if (!string.IsNullOrWhiteSpace(request.Filter))
            {
                query = query.Where(s => s.Name.ToLower().Contains(request.Filter.ToLower()));
            }

            return await GetPaginationAsync<Section, SectionDTO>(request, query);
        }
    }
}
