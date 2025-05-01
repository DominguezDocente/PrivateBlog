using System.ComponentModel.DataAnnotations;

namespace PrivateBlog.Web.Data.Entities
{
    public class PermissionDTO
    {
        public int Id { get; set; }

        [Display(Name = "Rol")]
        public string Name { get; set; } = null!;

        [Display(Name = "Descripción")]
        public string Description { get; set; } = null!;

        [Display(Name = "Módulo")]
        public string Module { get; set; } = null!;
    }
}
