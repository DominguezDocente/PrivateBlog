using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PrivateBlog.Web.Data.Entities
{
    public class Blog
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

        public bool IsPublished { get; set; } = false;

        public User Author { get; set; } = null!;
        public string AuthorId { get; set; } = null!;

        public Section Section { get; set; } = null!;
        public int SectionId { get; set; }
    }
}
