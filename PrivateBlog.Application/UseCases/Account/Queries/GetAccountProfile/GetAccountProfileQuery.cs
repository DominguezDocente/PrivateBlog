using PrivateBlog.Application.Utilities.Mediator;

namespace PrivateBlog.Application.UseCases.Account.Queries.GetAccountProfile
{
    public class GetAccountProfileQuery : IRequest<AccountProfileDTO>
    {
        public required string UserId { get; set; }
    }
}
