using System.ComponentModel.DataAnnotations;

namespace PrivateBlog.Web.DTOs.Sections
{
    public class CreateSectionDTO
    {
        [Required]
        [StringLength(64, MinimumLength = 3)]
        [Display(Name = "Sección")]
        public required string Name { get; set; }
    }
}
