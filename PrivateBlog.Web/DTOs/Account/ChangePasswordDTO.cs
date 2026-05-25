using System.ComponentModel.DataAnnotations;

namespace PrivateBlog.Web.DTOs.Account
{
    public class ChangePasswordDTO
    {
        [Required(ErrorMessage = "La contraseña actual es obligatoria.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña actual")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "La nueva contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "La nueva contraseña debe tener al menos 4 caracteres.")]
        [Display(Name = "Nueva contraseña")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "La confirmación es obligatoria.")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "La confirmación no coincide con la nueva contraseña.")]
        [Display(Name = "Confirmar nueva contraseña")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}
