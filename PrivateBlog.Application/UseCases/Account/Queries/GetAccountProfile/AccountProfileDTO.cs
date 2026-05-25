namespace PrivateBlog.Application.UseCases.Account.Queries.GetAccountProfile
{
    public class AccountProfileDTO
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public required string RoleName { get; set; }
    }
}
