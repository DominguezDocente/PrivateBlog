using System.ComponentModel.DataAnnotations;

namespace PrivateBlog.Web.DTOs
{
    public class AccountUserDTO
    {
        public Guid Id { get; set; }

        [Display(Name = "Documento")]
        [MaxLength(32, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Document { get; set; } = null!;

        [Display(Name = "Nombres")]
        [MaxLength(64, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [RegularExpression(@"^[a-zA-Z0-9áéíóúÁÉÍÓÚ\s]+$", ErrorMessage = "La cadena contiene caracteres especiales.")]
        public string FirstName { get; set; } = null!;

        [Display(Name = "Apellidos")]
        [MaxLength(64, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [RegularExpression(@"^[a-zA-Z0-9áéíóúÁÉÍÓÚ\s]+$", ErrorMessage = "La cadena contiene caracteres especiales.")]
        public string LastName { get; set; } = null!;
        public bool IsNew { get; set; }

        [Display(Name = "Teléfono")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string PhoneNumber { get; set; } = null!;

        public string Email { get; set; }

        [Display(Name = "Usuario")]
        public string FullName => $"{FirstName} {LastName}";
    }
}
