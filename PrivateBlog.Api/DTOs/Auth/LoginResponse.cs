namespace PrivateBlog.Api.DTOs.Auth
{
    public class LoginResponse
    {
        public required string AccessToken { get; set; }
        public required DateTime ExpiresAtUtc { get; set; }
        public required string UserId { get; set; }
        public required string Email { get; set; }
        public required string FullName { get; set; }
        public required string RoleName { get; set; }
    }
}
