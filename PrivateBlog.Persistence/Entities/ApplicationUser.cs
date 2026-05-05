using Microsoft.AspNetCore.Identity;
using PrivateBlog.Domain.Entities.Account;

namespace PrivateBlog.Persistence.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
    }
}
