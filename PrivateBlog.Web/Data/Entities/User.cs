using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PrivateBlog.Web.Data.Entities
{
    public class User : IdentityUser
    {
        [MaxLength(32)]
        [Required]
        public required string Document { get; set; }

        [MaxLength(64)]
        [Required]
        public required string FirstName { get; set; }

        [MaxLength(64)]
        [Required]
        public required string LastName { get; set; }

        public required Guid PrivateBlogRoleId { get; set; }

        public PrivateBlogRole? PrivateBlogRole { get; set; }

        public string? Photo { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}
