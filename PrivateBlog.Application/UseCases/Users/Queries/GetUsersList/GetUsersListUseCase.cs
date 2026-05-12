using PrivateBlog.Application.Contracts.Pagination;
using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.Utilities.Mediator;

namespace PrivateBlog.Application.UseCases.Users.Queries.GetUsersList
{
    public sealed class GetUsersListUseCase : IRequestHandler<GetUsersListQuery, PaginationResponse<UserListItemDTO>>
    {
        private readonly IUsersRepository _usersRepository;

        public GetUsersListUseCase(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task<PaginationResponse<UserListItemDTO>> Handle(GetUsersListQuery query)
        {
            (List<UserListItemDTO> items, int totalCount) = await _usersRepository.GetPagedListAsync(
                query.Pagination, query.NameFilter, query.RoleIdFilter);

            return PaginationResponse<UserListItemDTO>.Create(items, totalCount, query.Pagination);
        }
    }
}
