namespace PrivateBlog.Api.Services
{
    public interface IJwtTokenService
    {
        Task<JwtTokenResult> CreateTokenAsync(string userId, CancellationToken cancellationToken = default);
    }

    public sealed record JwtTokenResult(string AccessToken, DateTime ExpiresAtUtc);
}
