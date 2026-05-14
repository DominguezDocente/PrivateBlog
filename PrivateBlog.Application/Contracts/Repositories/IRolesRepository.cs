using PrivateBlog.Application.Contracts.Pagination;
using PrivateBlog.Application.UseCases.Roles.Queries.GetRolesList;
using PrivateBlog.Domain.Entities.Account;

namespace PrivateBlog.Application.Contracts.Repositories
{
    public interface IRolesRepository
    {
        Task<(List<RoleListItemDTO> Items, int TotalCount)> GetPagedListAsync(
            PaginationRequest request,
            string? nameFilter,
            CancellationToken cancellationToken = default);

        Task<Role?> GetByIdWithPermissionsAsync(Guid id, CancellationToken cancellationToken = default);

        Task<bool> ExistsByNameAsync(string name, Guid? excludeId = null, CancellationToken cancellationToken = default);

        Task CreateAsync(Role role, List<Guid> permissionIds, CancellationToken cancellationToken = default);

        Task UpdateAsync(Role role, List<Guid> permissionIds, CancellationToken cancellationToken = default);

        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

        Task<bool> HasUsersAsync(Guid roleId, CancellationToken cancellationToken = default);

        Task<List<Permission>> GetAllPermissionsAsync(CancellationToken cancellationToken = default);
    }
}
