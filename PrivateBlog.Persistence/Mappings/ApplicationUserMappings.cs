using PrivateBlog.Domain.Entities.Account;
using PrivateBlog.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Persistence.Mappings
{
    public static class ApplicationUserMappings
    {
        public static User ToDomainUser(this ApplicationUser user)
        {
            ArgumentNullException.ThrowIfNull(user);

            return User.Reconstitute(
                user.Id,
                user.RoleId,
                user.FirstName,
                user.LastName,
                user.UserName,
                user.Email,
                user.EmailConfirmed,
                user.PhoneNumber);
        }
    }
}
