using Microsoft.AspNetCore.Identity;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.DTOs;

namespace PrivateBlog.Web.Services.Abtractions
{
    public interface IUsersService
    {
        public Task<Response<IdentityResult>> AddUserAsync(User user, string password);
        public Task<Response<IdentityResult>> ConfirmUserAsync(User user, string token);
        public bool CurrentUserIsAuthenticaded();
        public Task<bool> CurrentUserIsAuthorizedAsync(string permission, string module);
        public Task<Response<string>> GenerateConfirmationTokenAsync(User user);
        public Task<User> GetUserByEmailasync(string email);
        public Task<Response<SignInResult>> LoginAsync(LoginDTO dto);
        public Task LogoutAsync();
        public Task<Response<AccountUserDTO>> UpdateUserAsync(AccountUserDTO dto);
    }
}
