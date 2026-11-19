namespace PrivateBlog.Web.DTOs.Account
{
    public class UserTokenDTO
    {
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
