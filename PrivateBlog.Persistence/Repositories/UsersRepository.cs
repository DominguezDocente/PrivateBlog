using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrivateBlog.Application.Contracts.Pagination;
using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.UseCases.Users.Queries.GetUsersList;
using PrivateBlog.Domain.Entities.Account;
using PrivateBlog.Domain.Exceptions;
using PrivateBlog.Persistence.Entities;
using PrivateBlog.Persistence.Extensions;

namespace PrivateBlog.Persistence.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly DataContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersRepository(DataContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task CreateAsync(User user, string password, CancellationToken cancellationToken = default)
        {
            ApplicationUser appUser = new ApplicationUser
            {
                Id = user.Id,
                FirstName = user.FisrtName,
                LastName = user.LastName,
                UserName = user.Email,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumber = user.PhoneNumber,
                RoleId = user.RoleId,
            };

            IdentityResult result = await _userManager.CreateAsync(appUser, password);

            if (!result.Succeeded)
            {
                string errors = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new BussinesRuleException($"Error al crear el usuario: {errors}");
            }
        }

        public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            ApplicationUser? appUser = await _userManager.FindByIdAsync(id);

            if (appUser is null)
            {
                throw new BussinesRuleException("El usuario no existe.");
            }

            IdentityResult result = await _userManager.DeleteAsync(appUser);

            if (!result.Succeeded)
            {
                string errors = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new BussinesRuleException($"Error al eliminar el usuario: {errors}");
            }
        }

        public async Task<User?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            ApplicationUser? appUser = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

            return User.Reconstitute(appUser.Id,
                                     appUser.FirstName,
                                     appUser.LastName,
                                     appUser.UserName,
                                     appUser.Email,
                                     appUser.EmailConfirmed,
                                     appUser.PhoneNumber,
                                     appUser.RoleId);
        }

        public async Task<(List<UserListItemDTO> Items, int TotalCount)> GetPagedListAsync(PaginationRequest request, 
                                                                                           string? nameFilter, 
                                                                                           Guid? roleIdFilter, 
                                                                                           CancellationToken cancellationToken = default)
        {
            IQueryable<ApplicationUser> query = _context.Users.Include(u => u.Role)
                                                              .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(nameFilter))
            {
                string term = nameFilter.Trim().ToLower();
                query = query.Where(u => u.FirstName.ToLower().Contains(term)
                                      || u.LastName.ToLower().Contains(term)
                                      || u.Email!.ToLower().Contains(term));
            }

            if (roleIdFilter.HasValue)
            {
                query = query.Where(u => u.RoleId == roleIdFilter.Value);
            }

            IQueryable<UserListItemDTO> projected = query
                .OrderBy(u => u.FirstName)
                .ThenBy(u => u.LastName)
                .Select(u => new UserListItemDTO
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email!,
                    RoleName = u.Role.Name,
                });

            (List<UserListItemDTO> items, int totalCount) = await projected.ToPagedListAsync(request, cancellationToken);

            return (items, totalCount);
        }

        public async Task<List<Role>> GetRolesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Roles.AsNoTracking()
                                       .OrderBy(r => r.Name)
                                       .ToListAsync(cancellationToken);
        }

        public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
        {
            ApplicationUser? appUser = await _userManager.FindByIdAsync(user.Id);

            if (appUser is null)
            {
                throw new BussinesRuleException("El usuario no existe.");
            }

            appUser.FirstName = user.FisrtName;
            appUser.LastName = user.LastName;
            appUser.Email = user.Email;
            appUser.UserName = user.Email;
            appUser.PhoneNumber = user.PhoneNumber;
            appUser.RoleId = user.RoleId;

            IdentityResult result = await _userManager.UpdateAsync(appUser);

            if (!result.Succeeded)
            {
                string errors = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new BussinesRuleException($"Error al actualizar el usuario: {errors}");
            }
        }
    }
}
