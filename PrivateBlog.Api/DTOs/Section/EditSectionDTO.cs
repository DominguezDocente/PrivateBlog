using System.ComponentModel.DataAnnotations;

namespace PrivateBlog.Api.DTOs.Section
{
    public class EditSectionDTO
    {
        [Required]
        [StringLength(64, MinimumLength = 3)]
        public required string Name { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
