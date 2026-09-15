using PrivateBlog.Web.Data.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace PrivateBlog.Web.Data.Entities
{
    public class Section : IId
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(32)]
        public required string Name { get; set; }

        [MaxLength(128)]
        public string? Description { get; set; }

        public bool IsHidden { get; set; } = false;
    }
}
