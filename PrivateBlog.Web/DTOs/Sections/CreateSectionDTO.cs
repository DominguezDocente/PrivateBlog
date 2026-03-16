using System.ComponentModel.DataAnnotations;

namespace PrivateBlog.Web.DTOs.Sections
{
    public class CreateSectionDTO
    {
        [Required]
        [StringLength(64)]
        public required string Name { get; set; }
    }
}
