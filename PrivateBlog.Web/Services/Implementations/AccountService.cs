using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Data;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.DTOs.Account;
using PrivateBlog.Web.DTOs.User;
using PrivateBlog.Web.Services.Abstractions;
using System.Security.Claims;

namespace PrivateBlog.Web.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUsersService _usersService;
        private readonly IHttpContextAccessor _httpContextAccesor;
        private readonly DataContext _context;

        public AccountService(UserManager<User> userManager, 
                              SignInManager<User> signInManager, 
                              IUsersService usersService, 
                              IHttpContextAccessor httpContextAccesor, 
                              DataContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _usersService = usersService;
            _httpContextAccesor = httpContextAccesor;
            _context = context;
        }

        public bool AccountUserIsAuthenticated()
        {
            ClaimsPrincipal? user = _httpContextAccesor.HttpContext?.User;
            return user?.Identity is not null && user.Identity.IsAuthenticated;
        }

        public async Task<bool> AccountUserIsAuthorizedAsync(string code)
        {
            ClaimsPrincipal? claimUser = _httpContextAccesor.HttpContext?.User;

            // Check identity (session)
            if (claimUser is null)
            {
                return false;
            }

            string email = claimUser.Identity!.Name!;

            Response<User> response = await _usersService.GetUserByEmailAsync(email);

            if (!response.IsSuccess || response.Result is null)
            {
                return false;
            }

            User user = response.Result;

            if (user.PrivateBlogRole.Name == Env.SUPER_ADMIN_ROLE_NAME)
            {
                return true;
            }

            return await _context.Permissions.Include(p => p.RolePermissions)
                                             .AnyAsync(p => p.Code == code
                                                         && p.RolePermissions.Any(rp => rp.PrivateBlogRoleId == user.PrivateBlogRoleId));
        }

        public async Task<bool> AccountUserIsSuperAdminAsync()
        {
            ClaimsPrincipal? claimUser = _httpContextAccesor.HttpContext?.User;

            // Check identity (session)
            if (claimUser is null)
            {
                return false;
            }

            string email = claimUser.Identity!.Name!;

            Response<User> response = await _usersService.GetUserByEmailAsync(email);

            if (!response.IsSuccess || response.Result is null)
            {
                return false;
            }

            User user = response.Result;

            return user.PrivateBlogRole.Name == Env.SUPER_ADMIN_ROLE_NAME;
        }

        public async Task<Response<IdentityResult>> ConfirmUserAsync(User user, string token)
        {
            IdentityResult result = await _userManager.ConfirmEmailAsync(user, token);

            return new Response<IdentityResult>
            {
                Result = result,
                IsSuccess = result.Succeeded
            };
        }

        public async Task<Response<string>> GenerateConfirmationTokenAsync(User user)
        {
            string result = await _userManager.GeneratePasswordResetTokenAsync(user);

            return Response<string>.Success(result);
        }

        public async Task<Response<SignInResult>> LoginAsync(LoginDTO dto)
        {
            SignInResult result = await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, false, false);

            return new Response<SignInResult>
            {
                Result = result,
                IsSuccess = result.Succeeded
            };
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<Response<AccountUserDTO>> UpdateAccountUserAsync(AccountUserDTO dto)
        {
            try
            {
                ClaimsPrincipal? claimUser = _httpContextAccesor.HttpContext?.User;

                // Check identity (session)
                if (claimUser is null)
                {
                    return Response<AccountUserDTO>.Failure();
                }

                string email = claimUser.Identity!.Name!;

                Response<User> response = await _usersService.GetUserByEmailAsync(email);

                if (!response.IsSuccess || response.Result is null)
                {
                    return Response<AccountUserDTO>.Failure();
                }

                User accountUser = response.Result;

                if (accountUser.Id != dto.Id.ToString())
                {
                    return Response<AccountUserDTO>.Failure();
                }

                UserDTO userDto = new UserDTO
                {
                    Id = dto.Id,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Phone = dto.Phone,
                    Photo = dto.Photo,
                    Document = dto.Document
                };

                Response<UserDTO> userDtoResponse = await _usersService.UpdateUserAsync(userDto);

                if (userDtoResponse.IsSuccess)
                {
                    return Response<AccountUserDTO>.Success(dto);
                }

                return Response<AccountUserDTO>.Failure(userDtoResponse.Message, userDtoResponse.Errors);

            }
            catch (Exception ex)
            {
                return Response<AccountUserDTO>.Failure(ex);
            }
        }
    }
}
