using System.ComponentModel.DataAnnotations;

namespace PrivateBlog.Web.DTOs.Account
{
    public class ChangePasswordDTO
    {
        [Display(Name = "Contraseña Actual")]
        [MinLength(4, ErrorMessage = "El campo {0} debe tener una longitud de {1} carácteres como minimo.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Display(Name = "Nueva Contraseña")]
        [MinLength(4, ErrorMessage = "El campo {0} debe tener una longitud de {1} carácteres como minimo.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Display(Name = "Nueva Contraseña Confirmación")]
        [MinLength(4, ErrorMessage = "El campo {0} debe tener una longitud de {1} carácteres como minimo.")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Las constraseñas no coinciden")]
        public string ConfirmPassword { get; set; }
    }
}
