using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Data;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.DTOs;
using ClaimsUser = System.Security.Claims.ClaimsPrincipal;

namespace PrivateBlog.Web.Services
{
    public interface IUsersService
    {
        public Task<IdentityResult> AddUserAsync(User user, string password);
        public Task<IdentityResult> ConfirmEmailAsync(User user, string token);
        public bool CurrentUserIsAuthenticated();
        public Task<bool> CurrentUserIsAuthorizedAsync(string permission, string module);
        public Task<string> GenerateEmailConfirmationTokenAsync(User user);
        Task<User?> GetCurrentUserAsync();
        public Task<User> GetUserAsync(string email);
        public Task<SignInResult> LoginAsync(LoginDTO dto);
        public Task LogoutAsync();
        public Task<int> UpdateUserAsync(AccountUserDTO user);
    }

    public class UsersService : IUsersService
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public UsersService(DataContext context, UserManager<User> userManager, SignInManager<User> signInManager, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }

        public bool CurrentUserIsAuthenticated()
        {
            ClaimsUser? user = _httpContextAccessor.HttpContext?.User;
            return user?.Identity != null && user.Identity.IsAuthenticated;
        }

        public async Task<bool> CurrentUserIsAuthorizedAsync(string permission, string module)
        {
            ClaimsUser? claimUser = _httpContextAccessor.HttpContext?.User;

            // Valida si hay sesión
            if (claimUser is null)
            {
                return false;
            }

            string? userName = claimUser.Identity.Name;

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

        public async Task<User?> GetCurrentUserAsync()
        {
            ClaimsUser? claimUser = _httpContextAccessor.HttpContext?.User;

            if (claimUser is null)
            {
                return null;
            }

            string? userName = claimUser.Identity.Name;

            User? user = await GetUserAsync(userName);

            return user;
        }

        public async Task<User> GetUserAsync(string email)
        {
            User? user = await _context.Users.Include(u => u.PrivateBlogRole)
                                             .FirstOrDefaultAsync(u => u.Email == email);

            return user;
        }

        public async Task<SignInResult> LoginAsync(LoginDTO dto)
        {
            return await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, false, false);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<int> UpdateUserAsync(AccountUserDTO dto)
        {
            User user = _mapper.Map<User>(dto);

            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded ? 1 : 0;

            //_context.Users.Update(user);
            //return await _context.SaveChangesAsync();
        }
    }
}
