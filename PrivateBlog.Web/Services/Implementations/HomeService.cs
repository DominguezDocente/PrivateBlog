using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Core.Pagination;
using PrivateBlog.Web.Data;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.DTOs.Blog;
using PrivateBlog.Web.DTOs.Section;
using PrivateBlog.Web.Services.Abstractions;
using ClaimsUser = System.Security.Claims.ClaimsPrincipal;

namespace PrivateBlog.Web.Services.Implementations
{
    public class HomeService : CustomQueryableOperationsService, IHomeService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly IUsersService _usersService;

        public HomeService(DataContext context,
                           IHttpContextAccessor httpContextAccessor,
                           IAccountService accountService,
                           IMapper mapper,
                           IUsersService usersService) : base(context, mapper)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _accountService = accountService;
            _mapper = mapper;
            _usersService = usersService;
        }

        // TODO: Transponse get user responsability to IAccountService
        public async Task<Response<BlogDTO>> GetBlogAsync(Guid id)
        {
            Blog? blog = await _context.Blogs.Include(b => b.Section)
                                                .ThenInclude(s => s.RoleSections)
                                             .FirstOrDefaultAsync(b => b.Id == id);

            if (blog is null)
            {
                return Response<BlogDTO>.Failure(new Exception($"El Blog con id: '{id}' no existe."));
            }

            ClaimsUser? claimsUser = _httpContextAccessor.HttpContext?.User;
            string? userName = claimsUser.Identity.Name;
            Response<User> userResponse = await _usersService.GetUserByEmailAsync(userName);

            User user = userResponse.Result;

            bool isAutorized = true;
            if (!await _accountService.AccountUserIsSuperAdminAsync())
            {
                isAutorized = blog.Section.RoleSections.Any(rs => rs.PrivateBlogRoleId == user.PrivateBlogRoleId);
            }

            if (!isAutorized)
            {
                return Response<BlogDTO>.Failure("No tiene autorización para consultar este blog");
            }

            BlogDTO dto = _mapper.Map<BlogDTO>(blog);
            return Response<BlogDTO>.Success(dto);
        }

        // TODO: Transponse get user responsability to IAccountService
        public async Task<Response<SectionDTO>> GetSectionAsync(Guid id, PaginationRequest request)
        {
            try
            {
                Section? section = await _context.Sections.Include(s => s.RoleSections)
                                                          .Where(s => !s.IsHidden && s.Id == id)
                                                          .FirstOrDefaultAsync();

                if (section is null)
                {
                    return Response<SectionDTO>.Failure(new Exception($"La sección con id: '{id}' no existe."));
                }

                ClaimsUser? claimsUser = _httpContextAccessor.HttpContext?.User;
                string? userName = claimsUser.Identity.Name;
                Response<User> userResponse = await _usersService.GetUserByEmailAsync(userName);

                User user = userResponse.Result;

                bool isAutorized = true;
                if (!await _accountService.AccountUserIsSuperAdminAsync())
                {
                    isAutorized = section.RoleSections.Any(rs => rs.PrivateBlogRoleId == user.PrivateBlogRoleId);
                }

                if (!isAutorized)
                {
                    return Response<SectionDTO>.Failure("No tiene autorización para consultar esta sección");
                }

                IQueryable<Blog> query = _context.Blogs.Where(b => b.SectionId == section.Id);

                query = query.Select(b => new Blog
                {
                    Id = b.Id,
                    Title = b.Title,
                    SectionId = b.SectionId,
                });

                if (!string.IsNullOrWhiteSpace(request.Filter))
                {
                    query = query.Where(s => s.Title.ToLower().Contains(request.Filter.ToLower()));
                }

                Response<PaginationResponse<BlogDTO>> paginationResponse = await GetPagedListAsync<BlogDTO, Blog>(request, query);
                if (!paginationResponse.IsSuccess)
                {
                    return Response<SectionDTO>.Failure(paginationResponse.Message, paginationResponse.Errors);
                }

                SectionDTO dto = new SectionDTO
                {
                    Id = section.Id,
                    Name = section.Name,
                    Description = section.Description,
                    PagedBlogs = paginationResponse.Result
                };

                return Response<SectionDTO>.Success(dto);
            }
            catch (Exception ex)
            {
                return Response<SectionDTO>.Failure(ex);
            }
        }

        // TODO: Transponse get user responsability to IAccountService
        public async Task<Response<PaginationResponse<SectionDTO>>> GetSectionsAsync(PaginationRequest request)
        {
            ClaimsUser? claimsUser = _httpContextAccessor.HttpContext?.User;
            string? userName = claimsUser.Identity.Name;
            Response<User> userResponse = await _usersService.GetUserByEmailAsync(userName);

            User user = userResponse.Result;

            IQueryable<Section> query = _context.Sections.Include(s => s.RoleSections)
                                                         .Where(s => !s.IsHidden);

            if (!await _accountService.AccountUserIsSuperAdminAsync())
            {
                query = query.Where(s => s.RoleSections.Any(rs => rs.PrivateBlogRoleId == user.PrivateBlogRoleId));
            }

            if (!string.IsNullOrWhiteSpace(request.Filter))
            {
                query = query.Where(s => s.Name.ToLower().Contains(request.Filter.ToLower()));
            }

            return await GetPagedListAsync<SectionDTO, Section>(request, query);
        }
    }
}
