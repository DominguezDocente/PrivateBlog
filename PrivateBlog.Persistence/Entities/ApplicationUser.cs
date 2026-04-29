using Microsoft.AspNetCore.Identity;
using PrivateBlog.Domain.Entities.Users;

namespace PrivateBlog.Persistence.Entities
{
    /// <summary>
    /// Usuario persistido por ASP.NET Identity. El modelo de dominio es <see cref="PrivateBlog.Domain.Entities.Users.User"/> (véase <c>Mapping.ApplicationUserMappings.ToDomainUser</c>).
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public Guid RoleId { get; set; }
        public Role Role { get; set; } = null!;
    }
}
