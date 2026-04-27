using Microsoft.AspNetCore.Identity;

namespace PrivateBlog.Persistence.Entities
{
    /// <summary>
    /// Usuario persistido por ASP.NET Identity. El modelo de dominio es <see cref="PrivateBlog.Domain.Entities.Users.User"/> (véase <c>Mapping.ApplicationUserMappings.ToDomainUser</c>).
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
    }
}
