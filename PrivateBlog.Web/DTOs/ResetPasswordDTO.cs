using System.ComponentModel.DataAnnotations;

namespace PrivateBlog.Web.DTOs
{
    public class ResetPasswordDTO
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [EmailAddress(ErrorMessage = "Debe introducir un email válido.")]
        public string Email { get; set; } = null!;

        [Display(Name = "Nueva contraseña")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [MinLength(4, ErrorMessage = "El campo {0} debe tener una longitud mínima de {1} carácteres")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Display(Name = "Confirmación de contraseña")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [MinLength(4, ErrorMessage = "El campo {0} debe tener una longitud mínima de {1} carácteres")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "La nueva contrasela y la confirmación no son iguales.")]
        public string ConfirmPassword { get; set; } = null!;

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Token { get; set; } = null!;
    }
}
