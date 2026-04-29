using PrivateBlog.Application.Contracts.Account;

namespace PrivateBlog.Application.Contracts.Repositories
{
    /// <summary>
    /// Lecturas relacionadas con usuarios Identity y roles/permisos de aplicación.
    /// </summary>
    public interface IAccountRepository
    {
        Task<bool> UserHasPermissionAsync(
            string userId,
            string permissionCode,
            CancellationToken cancellationToken = default);

        Task<UserHeaderInfoDTO?> GetUserHeaderInfoAsync(
            string userId,
            CancellationToken cancellationToken = default);
    }
}
