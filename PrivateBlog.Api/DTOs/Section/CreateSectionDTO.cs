using System.ComponentModel.DataAnnotations;

namespace PrivateBlog.Api.DTOs.Section
{
    public class CreateSectionDTO
    {
        [Required]
        [StringLength(64, MinimumLength = 4)]
        public required string Name { get; set; }
    }
}
