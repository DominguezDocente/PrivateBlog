using System.ComponentModel.DataAnnotations;

namespace PrivateBlog.Web.DTOs.Sections
{
    public class EditSectionDTO
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(64, MinimumLength = 3)]
        [Display(Name = "Sección")]
        public required string Name { get; set; }

        [Display(Name = "Activa")]
        public bool IsActive { get; set; }
    }
}
