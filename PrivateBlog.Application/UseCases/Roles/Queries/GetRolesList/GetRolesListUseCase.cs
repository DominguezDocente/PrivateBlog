using PrivateBlog.Application.Contracts.Pagination;
using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.Utilities.Mediator;

namespace PrivateBlog.Application.UseCases.Roles.Queries.GetRolesList
{
    public sealed class GetRolesListUseCase : IRequestHandler<GetRolesListQuery, PaginationResponse<RoleListItemDTO>>
    {
        private readonly IRolesRepository _rolesRepository;

        public GetRolesListUseCase(IRolesRepository rolesRepository)
        {
            _rolesRepository = rolesRepository;
        }

        public async Task<PaginationResponse<RoleListItemDTO>> Handle(GetRolesListQuery query)
        {
            (List<RoleListItemDTO> items, int totalCount) = await _rolesRepository.GetPagedListAsync(
                query.Pagination, query.NameFilter);

            return PaginationResponse<RoleListItemDTO>.Create(items, totalCount, query.Pagination);
        }
    }
}
