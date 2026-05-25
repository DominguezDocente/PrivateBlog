using PrivateBlog.Application.UseCases.Account.Commands.Login;
using PrivateBlog.Application.UseCases.Account.Queries.GetAccountUserInfo;
using PrivateBlog.Application.UseCases.Account.Queries.GetProfile;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.Contracts.Repositories
{
    public interface IAccountRepository
    {
        Task<AccountSignInResult> SignInAsync(string userName, string password, bool rememberMe, CancellationToken cancellationToken = default);

        Task SignOutAsync(CancellationToken cancellationToken = default);

        Task<UserAccountInfoDTO> GetUserInfoAsync(string userId, CancellationToken cancellationToken = default);

        Task<bool> UserHasPermissionAsync(string userId, string permissionCode, CancellationToken cancellationToken = default);

        Task<AccountProfileDTO> GetProfileAsync(string userId, CancellationToken cancellationToken = default);

        Task UpdateProfileAsync(string userId, string firstName, string lastName, string? phoneNumber, CancellationToken cancellationToken = default);

        Task ChangePasswordAsync(string userId, string currentPassword, string newPassword, CancellationToken cancellationToken = default);
    }
}
