using System.ComponentModel.DataAnnotations;

namespace PrivateBlog.Web.DTOs.Blogs
{
    public sealed class CreateBlogDTO : IValidatableObject
    {
        [Display(Name = "Título")]
        [Required(ErrorMessage = "El título es obligatorio.")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "El título debe tener entre 3 y 200 caracteres.")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Contenido (HTML)")]
        public string Content { get; set; } = string.Empty;

        [Display(Name = "Sección")]
        public Guid SectionId { get; set; }

        [Display(Name = "Publicado")]
        public bool IsPublished { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (SectionId == Guid.Empty)
            {
                yield return new ValidationResult(
                    "Debe seleccionar una sección.",
                    new[] { nameof(SectionId) });
            }
        }
    }
}
