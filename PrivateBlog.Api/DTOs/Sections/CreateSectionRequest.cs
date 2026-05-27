using System.ComponentModel.DataAnnotations;

namespace PrivateBlog.Api.DTOs.Sections
{
    public class CreateSectionRequest
    {
        [Required]
        [StringLength(64, MinimumLength = 3)]
        public required string Name { get; set; }
    }
}
