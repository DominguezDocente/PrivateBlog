using PrivateBlog.Application.Contracts.Pagination;
using PrivateBlog.Application.UseCases.Users.Queries.GetUsersList;
using PrivateBlog.Domain.Entities.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.Contracts.Repositories
{
    public interface IUsersRepository
    {
        Task<(List<UserListItemDTO> Items, int TotalCount)> GetPagedListAsync(
            PaginationRequest request,
            string? nameFilter,
            Guid? roleIdFilter,
            CancellationToken cancellationToken = default);

        Task<User?> GetByIdAsync(string id, CancellationToken cancellationToken = default);

        Task CreateAsync(User user, string password, CancellationToken cancellationToken = default);

        Task UpdateAsync(User user, CancellationToken cancellationToken = default);

        Task DeleteAsync(string id, CancellationToken cancellationToken = default);

        Task<List<Role>> GetRolesAsync(CancellationToken cancellationToken = default);
    }
}
