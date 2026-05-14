using Microsoft.EntityFrameworkCore;
using PrivateBlog.Application.Contracts.Pagination;
using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.UseCases.Roles.Queries.GetRolesList;
using PrivateBlog.Domain.Entities.Account;
using PrivateBlog.Domain.Exceptions;
using PrivateBlog.Persistence.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Persistence.Repositories
{
    public class RolesRepository : IRolesRepository
    {
        private readonly DataContext _context;

        public RolesRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<(List<RoleListItemDTO> Items, int TotalCount)> GetPagedListAsync(
            PaginationRequest request,
            string? nameFilter,
            CancellationToken cancellationToken = default) 
        {
            IQueryable<Role> query = _context.Roles.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(nameFilter))
            {
                string term = nameFilter.Trim().ToLower();
                query = query.Where(r => r.Name.ToLower().Contains(term));
            }

            IQueryable<RoleListItemDTO> projected = query
                .OrderBy(r => r.Name)
                .Select(r => new RoleListItemDTO
                {
                    Id = r.Id,
                    Name = r.Name,
                    PermissionCount = r.RolePermissions.Count,
                    PermissionIds = r.RolePermissions.Select(rp => rp.PermissionId).ToList()
                });

            (List<RoleListItemDTO> items, int totalCount) = await projected.ToPagedListAsync(request, cancellationToken);

            return (items, totalCount);
        }

        public async Task<Role?> GetByIdWithPermissionsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Roles.Include(r => r.RolePermissions)
                                       .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }

        public async Task<bool> ExistsByNameAsync(string name, Guid? excludeId = null, CancellationToken cancellationToken = default)
        {
            IQueryable<Role> query = _context.Roles.Where(r => r.Name.ToLower() == name.ToLower());

            if (excludeId.HasValue)
            {
                query = query.Where(r => r.Id != excludeId.Value);
            }

            return await query.AnyAsync(cancellationToken);
        }

        public async Task<List<Permission>> GetAllPermissionsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Permissions.AsNoTracking()
                                             .OrderBy(p => p.Module)
                                             .ThenBy(p => p.Description)
                                             .ToListAsync(cancellationToken);
        }

        public async Task<bool> HasUsersAsync(Guid roleId, CancellationToken cancellationToken = default)
        {
            return await _context.Users.AnyAsync(u => u.RoleId == roleId, cancellationToken);
        }

        public async Task UpdateAsync(Role role, List<Guid> permissionIds, CancellationToken cancellationToken = default)
        {
            List<RolePermission> existing = await _context.RolePermissions.Where(rp => rp.RoleId == role.Id)
                                                                          .ToListAsync(cancellationToken);

            _context.RolePermissions.RemoveRange(existing);

            foreach (Guid permissionId in permissionIds)
            {
                _context.RolePermissions.Add(new RolePermission(role.Id, permissionId));
            }

            _context.Roles.Update(role);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task CreateAsync(Role role, List<Guid> permissionIds, CancellationToken cancellationToken = default)
        {
            //using (var transaction = await _context.Database.BeginTransactionAsync()) 
            //{
            //    try
            //    {
            //        await _context.Roles.AddAsync(role);
            //        await _context.SaveChangesAsync(cancellationToken);

            //        foreach (Guid permissionId in permissionIds)
            //        {
            //            _context.RolePermissions.Add(new RolePermission(role.Id, permissionId));
            //        }

            //        await _context.SaveChangesAsync(cancellationToken);
            //        await transaction.CommitAsync(cancellationToken);
            //    }
            //    catch
            //    {
            //        await transaction.RollbackAsync(cancellationToken);
            //        throw;
            //    }
            //}

            await _context.Roles.AddAsync(role);

            foreach (Guid permissionId in permissionIds)
            {
                _context.RolePermissions.Add(new RolePermission(role.Id, permissionId));
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            Role? role = await _context.Roles.FindAsync([id], cancellationToken);

            if (role is null)
            {
                throw new BussinesRuleException("El rol no existe.");
            }

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
