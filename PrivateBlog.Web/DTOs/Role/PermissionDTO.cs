using System.ComponentModel.DataAnnotations;

namespace PrivateBlog.Web.DTOs.Role
{
    public class PermissionDTO
    {
        public Guid Id { get; set; }

        [Display(Name = "Código")]
        public string Code { get; set; }

        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Display(Name = "Módulo")]
        public string Module { get; set; }
    }
}
