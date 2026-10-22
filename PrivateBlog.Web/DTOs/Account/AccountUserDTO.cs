using Microsoft.AspNetCore.Mvc.Rendering;
using PrivateBlog.Web.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace PrivateBlog.Web.DTOs.Account
{
    public class AccountUserDTO
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(32, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres")]
        [Display(Name = "Documento")]
        public required string Document { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(64, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres")]
        [Display(Name = "Nombres")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(64, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres")]
        [Display(Name = "Apellidos")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(16, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres")]
        [Display(Name = "Teléfono")]
        public required string Phone { get; set; }

        public string? Email { get; set; }

        public string? RoleName { get; set; } = null!;

        public string? Photo { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}
