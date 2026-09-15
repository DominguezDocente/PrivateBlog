using System.ComponentModel.DataAnnotations;

namespace PrivateBlog.Web.DTOs.Section
{
    public class CreateSectionDTO
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(32, ErrorMessage = "El campo {0} debe tener como máximo {1} caracteres.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(128, ErrorMessage = "El campo {0} debe tener como máximo {1} caracteres.")]
        public string? Description { get; set; }

        public bool IsHidden { get; set; } = false;
    }
}
