using PrivateBlog.Application.Contracts.Account;
using PrivateBlog.Application.Utils.Mediator;

namespace PrivateBlog.Application.UseCases.Account.Queries.GetUserHeaderInfo
{
    public sealed class GetUserHeaderInfoQuery : IRequest<UserHeaderInfoDTO?>
    {
        public required string UserId { get; init; }
    }
}
