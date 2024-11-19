using Microsoft.EntityFrameworkCore;
using PrivateBlog.Web.Data;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.DTOs;

namespace PrivateBlog.Web.Helpers
{
    public interface IConverterHelper
    {
        public Blog ToBlog(BlogDTO dto);
        public Task<BlogDTO> ToBlogDTO(Blog result);
        PrivateBlogRole ToRole(PrivateBlogRoleDTO dto);
        public Task<PrivateBlogRoleDTO> ToRoleDTOAsync(PrivateBlogRole role);
        public User ToUser(UserDTO dto);
        public Task<UserDTO> ToUserDTOAsync(User user, bool isNew = true);
    }

    public class ConverterHelper : IConverterHelper
    {
        private readonly ICombosHelper _combosHelper;
        private readonly DataContext _context;

        public ConverterHelper(ICombosHelper combosHelper, DataContext context)
        {
            _combosHelper = combosHelper;
            _context = context;
        }

        public Blog ToBlog(BlogDTO dto)
        {
            return new Blog
            {
                Content = dto.Content,
                Id = dto.Id,
                IsPublished = dto.IsPublished,
                SectionId = dto.SectionId,
                Title = dto.Title,
            };
        }

        public async Task<BlogDTO> ToBlogDTO(Blog blog)
        {
            return new BlogDTO
            {
                Content = blog.Content,
                Id = blog.Id,
                IsPublished = blog.IsPublished,
                SectionId = blog.SectionId,
                Title = blog.Title,
                Sections= await _combosHelper.GetComboSections()
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

            List<SectionForDTO> sections = await _context.Sections.Select(p => new SectionForDTO
            {
                Id = p.Id,
                Name = p.Name,
                Selected = _context.RoleSections.Any(rs => rs.SectionId == p.Id && rs.RoleId == role.Id)
            }).ToListAsync();

            return new PrivateBlogRoleDTO
            {
                Id = role.Id,
                Name = role.Name,
                Permissions = permissions,
                Sections = sections
            };
        }

        public User ToUser(UserDTO dto)
        {
            return new User
            {
                Id = dto.Id.ToString(),
                Document = dto.Document,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                UserName = dto.Email,
                PrivateBlogRoleId = dto.PrivateBlogRoleId,
                PhoneNumber = dto.PhoneNumber,
            };
        }

        public async Task<UserDTO> ToUserDTOAsync(User user, bool isNew = true)
        {
            return new UserDTO
            {
                Id = isNew ? Guid.NewGuid() : Guid.Parse(user.Id),
                Document = user.Document,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PrivateBlogRoles = await _combosHelper.GetComboPrivateBlogRolesAsync(),
                PrivateBlogRoleId = user.PrivateBlogRoleId,
                PhoneNumber = user.PhoneNumber
            };
        }
    }
}
