using Microsoft.AspNetCore.Mvc.Rendering;
using PrivateBlog.Web.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace PrivateBlog.Web.DTOs.User
{
    public class UserDTO
    {
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

        [MaxLength(64, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres")]
        [Display(Name = "Correo")]
        [EmailAddress(ErrorMessage = "El campo {0} debe ser un Email válido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(16, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres")]
        [Display(Name = "Teléfono")]
        public required string Phone { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Rol")]
        public Guid PrivateBlogRoleId { get; set; }

        public PrivateBlogRole? PrivateBlogRole { get; set; }

        public IEnumerable<SelectListItem>? PrivateBlogRoles { get; set; }

        public string? Photo { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}
