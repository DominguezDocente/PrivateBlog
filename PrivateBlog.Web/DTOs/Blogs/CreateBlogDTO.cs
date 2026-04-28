using System.ComponentModel.DataAnnotations;

namespace PrivateBlog.Web.DTOs.Blogs
{
    public class CreateBlogDTO
    {
        [Display(Name = "Título")]
        [Required(ErrorMessage = "El título es obligatorio.")]
        [StringLength(128, MinimumLength = 3, ErrorMessage = "El título debe tener entre 3 y 128 caracteres.")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Contenido (HTML)")]
        [MinLength(8, ErrorMessage = "El contenido debe tener al menos 8 caracteres.")]
        public string Content { get; set; } = string.Empty;

        [Display(Name = "Sección")]
        public Guid SectionId { get; set; }

        [Display(Name = "Publicado")]
        public bool IsPublished { get; set; }
    }
}
