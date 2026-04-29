using PrivateBlog.Domain.Entities.Users;
using PrivateBlog.Persistence.Entities;

namespace PrivateBlog.Persistence.Mapping
{
    public static class ApplicationUserMappings
    {
        public static User ToDomainUser(this ApplicationUser user)
        {
            ArgumentNullException.ThrowIfNull(user);

            return User.Reconstitute(
                user.Id,
                user.RoleId,
                user.Email ?? string.Empty,
                user.UserName ?? string.Empty,
                user.EmailConfirmed,
                user.PhoneNumber,
                user.LockoutEnabled,
                user.LockoutEnd);
        }
    }
}
