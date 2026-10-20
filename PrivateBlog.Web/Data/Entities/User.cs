using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PrivateBlog.Web.Data.Entities
{
    public class User : IdentityUser
    {
        [Required]
        [MaxLength(32)]
        public required string Document { get; set; }

        [Required]
        [MaxLength(64)]
        public required string FirstName { get; set; }

        [Required]
        [MaxLength(64)]
        public required string LastName { get; set; }

        public required Guid PrivateBlogRoleId { get; set; }

        public PrivateBlogRole? PrivateBlogRole { get; set; }

        public string? Photo { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}
