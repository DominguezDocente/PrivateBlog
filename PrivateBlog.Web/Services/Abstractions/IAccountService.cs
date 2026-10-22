using Microsoft.AspNetCore.Identity;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.DTOs.Account;

namespace PrivateBlog.Web.Services.Abstractions
{
    public interface IAccountService
    {
        public Task<Response<IdentityResult>> ConfirmUserAsync(User user, string token);
        public Task<Response<string>> GenerateConfirmationTokenAsync(User user);
        public Task<Response<SignInResult>> LoginAsync(LoginDTO dto);
        public Task LogoutAsync();
        public Task<Response<AccountUserDTO>> UpdateAccountUserAsync(AccountUserDTO dto);
    }
}
