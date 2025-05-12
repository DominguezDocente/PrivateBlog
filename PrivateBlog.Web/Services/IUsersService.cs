using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
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
        Task<bool> CheckPasswordAsync(User user, string currentPassword);
        public Task<IdentityResult> ConfirmEmailAsync(User user, string token);
        public bool CurrentUserIsAuthenticated();
        public Task<bool> CurrentUserIsAuthorizedAsync(string permission, string module);
        public Task<string> GenerateEmailConfirmationTokenAsync(User user);
        Task<string> GeneratePasswordResetTokenAsync(User user);
        Task<User?> GetCurrentUserAsync();
        public Task<User> GetUserAsync(string email);
        public Task<SignInResult> LoginAsync(LoginDTO dto);
        public Task LogoutAsync();
        Task<IdentityResult> ResetPasswordAsync(User user, string resetToken, string newPassword);
        public Task<int> UpdateUserAsync(AccountUserDTO user);
    }

    public class UsersService : IUsersService
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly string _container = "users";
        private readonly IStorageService _storageService;

        public UsersService(DataContext context, UserManager<User> userManager, SignInManager<User> signInManager, IHttpContextAccessor httpContextAccessor, IMapper mapper, IStorageService storageService)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _storageService = storageService;
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

        public async Task<User?> GetUserAsync(Guid id)
        {
            return await _context.Users.Include(u => u.PrivateBlogRole)
                                       .FirstOrDefaultAsync(u => u.Id == id.ToString());
        }

        public async Task<int> UpdateUserAsync(AccountUserDTO dto)
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
                    user.Photo = await _storageService.SaveFileAsync(content, extension, _container, dto.Photo.ContentType);
                }
            }

            //var result = await _userManager.UpdateAsync(user);

            //return result.Succeeded ? 1 : 0;

            _context.Users.Update(user);
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> CheckPasswordAsync(User user, string currentPassword)
        {
            return await _userManager.CheckPasswordAsync(user, currentPassword);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<IdentityResult> ResetPasswordAsync(User user, string resetToken, string newPassword)
        {
            return await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
        }
    }
}
