namespace PrivateBlog.Application.Contracts.Authentication
{
    /// <summary>
    /// Abstrae el inicio/cierre de sesión para los casos de uso (implementación con Identity en infraestructura).
    /// </summary>
    public interface IAccountService
    {
        Task<AccountSignInResult> SignInWithPasswordAsync(
            string email,
            string password,
            bool rememberMe,
            CancellationToken cancellationToken = default);

        Task SignOutAsync(CancellationToken cancellationToken = default);
    }
}
