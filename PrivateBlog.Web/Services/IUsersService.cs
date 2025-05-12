using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Core.Pagination;
using PrivateBlog.Web.Data;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.DTOs;
using PrivateBlog.Web.Helpers;
using Serilog;
using ClaimsUser = System.Security.Claims.ClaimsPrincipal;

namespace PrivateBlog.Web.Services
{
    public interface IUsersService
    {
        public Task<IdentityResult> AddUserAsync(User user, string password);
        public bool CurrentUserIsAuthenticated();
        public Task<bool> CurrentUserIsAuthorizedAsync(string permission, string module);
        public Task<IdentityResult> ConfirmEmailAsync(User user, string token);
        public Task<string> GenerateEmailConfirmationTokenAsync(User user);
        public Task<User> GetUserAsync(string email);
        public Task<SignInResult> LoginAsync(LoginDTO dto);
        public Task LogoutAsync();
        public Task<Response<PaginationResponse<UserDTO>>> GetPaginationAsync(PaginationRequest request);
        public Task<Response<UserDTO>> CreateAsync(UserDTO dto);
        public Task<User?> GetUserAsync(Guid id);
        public Task<Response<UserDTO>> UpdateUserAsync(UserDTO dto);
        public Task<int> UpdateUserAsync(AccountUserDTO dto);
    }

    public class UsersService : CustomQueryableOperations, IUsersService
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IStorageService _localStorageService;
        private readonly IStorageService _azureStorageService;
        private readonly string _container = "users";

        public UsersService(DataContext context,
                            UserManager<User> userManager,
                            SignInManager<User> signInManager,
                            IHttpContextAccessor httpContextAccessor,
                            IMapper mapper,
                            [FromKeyedServices("local")] IStorageService localStorageService,
                            [FromKeyedServices("azure")] IStorageService azureStorageService)
            : base(context, mapper)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _localStorageService = localStorageService;
            _azureStorageService = azureStorageService;
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<Response<UserDTO>> CreateAsync(UserDTO dto)
        {
            try
            {
                User user = _mapper.Map<User>(dto);
                user.Id = Guid.NewGuid().ToString();

                IdentityResult result = await AddUserAsync(user, dto.Document);

                string token = await GenerateEmailConfirmationTokenAsync(user);
                await ConfirmEmailAsync(user, token);

                return ResponseHelper<UserDTO>.MakeResponseSuccess(_mapper.Map<UserDTO>(user), "Usuario creado con éxito");
            }
            catch(Exception ex)
            {
                return ResponseHelper<UserDTO>.MakeResponseFail(ex);
            }
        }

        public bool CurrentUserIsAuthenticated()
        {
            ClaimsUser? user = _httpContextAccessor.HttpContext?.User;
            return user?.Identity != null && user.Identity.IsAuthenticated;
        }

        public async Task<bool> CurrentUserIsAuthorizedAsync(string permission, string module)
        {
            ClaimsUser? claimsUser = _httpContextAccessor.HttpContext?.User;
        
            // Valida si hay sesión
            if (claimsUser is null)
            {
                return false;
            }

            string? userName = claimsUser.Identity!.Name;

            User? user = await GetUserAsync(userName);

            if (user is null)
            {
                return false;
            }    
            
            if (user.PrivateBlogRole.Name == Env.SUPER_ADMIN_ROLE_NAME)
            {
                return true;
            }

            return await _context.Permissions.Include(p => p.RolePermissions)
                                             .AnyAsync(p => (p.Module == module && p.Name == permission)
                                                            && p.RolePermissions.Any(rp => rp.RoleId == user.PrivateBlogRoleId));
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<Response<PaginationResponse<UserDTO>>> GetPaginationAsync(PaginationRequest request)
        {

            IQueryable<User> query = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(request.Filter))
            {
                query = query.Where(b => b.FirstName.ToLower().Contains(request.Filter.ToLower())
                                      || b.LastName.ToLower().Contains(request.Filter.ToLower())
                                      || b.Document.Contains(request.Filter)
                                      || b.Email.ToLower().Contains(request.Filter.ToLower())
                                      || b.PhoneNumber.Contains(request.Filter));
            }

            return await GetPaginationAsync<User, UserDTO>(request, query);
        }

        public async Task<User> GetUserAsync(string email)
        {
            User? user = await _context.Users.Include(u => u.PrivateBlogRole)
                                             .FirstOrDefaultAsync(u => u.Email == email);

            return user;
        }

        public async Task<User?> GetUserAsync(Guid id)
        {
            return await _context.Users.Include(u => u.PrivateBlogRole)
                                       .FirstOrDefaultAsync(u => u.Id == id.ToString());
        }

        public async Task<SignInResult> LoginAsync(LoginDTO dto)
        {
            return await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, false, false);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<Response<UserDTO>> UpdateUserAsync(UserDTO dto)
        {
            try
            {
                Guid id = Guid.Parse(dto.Id!);
                User user = await GetUserAsync(id);
                user.PhoneNumber = dto.PhoneNumber;
                user.Document = dto.Document;
                user.FirstName = dto.FirstName;
                user.LastName = dto.LastName;
                user.PrivateBlogRoleId = dto.PrivateBlogRoleId;

                _context.Users.Update(user);

                await _context.SaveChangesAsync();

                return ResponseHelper<UserDTO>.MakeResponseSuccess(dto, "Usuario actualizado con éxito");
            }
            catch(Exception ex)
            {
                return ResponseHelper<UserDTO>.MakeResponseFail(ex);
            }
        }

        public async Task<int> UpdateUserAsync(AccountUserDTO dto)
        {
            try
            {
                User user = await GetUserAsync(dto.Id);
                user.PhoneNumber = dto.PhoneNumber;
                user.Document = dto.Document;
                user.FirstName = dto.FirstName;
                user.LastName = dto.LastName;

                if (dto.Photo is not null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        await dto.Photo.CopyToAsync(ms);
                        byte[] content = ms.ToArray();
                        string extension = Path.GetExtension(dto.Photo.FileName);
                        user.Photo = await _localStorageService.SaveFileAsync(content, extension, _container, dto.Photo.ContentType);
                    }
                }

                _context.Users.Update(user);

                return await _context.SaveChangesAsync();
            } 
            
            catch(Exception ex)
            {
                Log.Error(ex.Message);
                return 0;
            }
        }
    }
}
