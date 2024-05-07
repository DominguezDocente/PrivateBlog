using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using PrivateBlog.Web.Data.Entities;

namespace PrivateBlog.Web.DTOs
{
    public class BlogDTO
    {
        public int Id { get; set; }

        [Display(Name = "Titulo")]
        [MaxLength(64, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Title { get; set; } = null!;

        [Display(Name = "Descripción")]
        [Column(TypeName = "varchar(MAX)")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Description { get; set; } = null!;

        public Section? Section { get; set; }

        [Display(Name = "Sección")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una sección.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int SectionId { get; set; }

        public IEnumerable<SelectListItem>? Sections { get; set; }

        public User? Author { get; set; }
        public string? AuthorId { get; set; }

        public bool IsPublished { get; set; } = false;

    }
}