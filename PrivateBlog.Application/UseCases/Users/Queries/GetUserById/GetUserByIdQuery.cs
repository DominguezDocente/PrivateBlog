using PrivateBlog.Application.Utilities.Mediator;

namespace PrivateBlog.Application.UseCases.Users.Queries.GetUserById
{
    public sealed class GetUserByIdQuery : IRequest<UserDetailDTO>
    {
        public required string Id { get; init; }
    }
}
