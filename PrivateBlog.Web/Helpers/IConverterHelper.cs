using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.DTOs;

namespace PrivateBlog.Web.Helpers
{
    public interface IConverterHelper
    {
        public Blog ToBlog(BlogDTO dto);
        public Task<BlogDTO> ToBlogDTO(Blog result);
        public User ToUser(UserDTO dto);
    }

    public class ConverterHelper : IConverterHelper
    {
        private readonly ICombosHelper _combosHelper;

        public ConverterHelper(ICombosHelper combosHelper)
        {
            _combosHelper = combosHelper;
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
    }
}
