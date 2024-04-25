using Microsoft.EntityFrameworkCore;
using PrivateBlog.Web.Data;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.DTOs;

namespace PrivateBlog.Web.Helpers
{
    public interface IConverterHelper
    {
        public AccountUserDTO ToAccountDTO(User user);
        public PrivateBlogRole ToRole(PrivateBlogRoleDTO dto);
        public Task<PrivateBlogRoleDTO> ToRoleDTOAsync(PrivateBlogRole role);
    }

    public class ConverterHelper : IConverterHelper
    {
        private readonly DataContext _context;

        public ConverterHelper(DataContext context)
        {
            _context = context;
        }

        public AccountUserDTO ToAccountDTO(User user)
        {
            return new AccountUserDTO
            {
                Id = Guid.Parse(user.Id),
                Document = user.Document,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
            };
        }

        public PrivateBlogRole ToRole(PrivateBlogRoleDTO dto)
        {
            return new PrivateBlogRole
            {
                Id = dto.Id,
                Name = dto.Name,
            };
        }

        public async Task<PrivateBlogRoleDTO> ToRoleDTOAsync(PrivateBlogRole role)
        {
            List<PermissionForDTO> permissions = await _context.Permissions.Select(p => new PermissionForDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Module = p.Module,
                Selected = _context.RolePermissions.Any(rp => rp.PermissionId == p.Id && rp.RoleId == role.Id)

            }).ToListAsync();

            return new PrivateBlogRoleDTO
            {
                Id = role.Id,
                Name = role.Name,
                Permissions = permissions,
            };
        }
    }
}
