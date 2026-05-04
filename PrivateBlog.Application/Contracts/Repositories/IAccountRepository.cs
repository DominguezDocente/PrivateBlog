using PrivateBlog.Application.UseCases.Account.Commands.Login;
using PrivateBlog.Application.UseCases.Account.Queries.GetAccountUserInfo;

namespace PrivateBlog.Application.Contracts.Repositories
{
    public interface IAccountRepository
    {
        Task<UserAccountInfoDTO?> GetUserInfoAsync(string userId, CancellationToken cancellationToken);

        Task<bool> UserHasPermissionAsync(string userId, string permissionCode, CancellationToken cancellationToken = default);

        Task<AccountSignInResult> SignInAsync(string userName, string password, bool rememberMe, CancellationToken cancellationToken = default);

        Task SignOutAsync(CancellationToken cancellationToken = default);
    }
}
