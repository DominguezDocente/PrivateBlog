using PrivateBlog.Web.Data.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrivateBlog.Web.Data.Entities
{
    public class Blog : IId
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(128)]
        [Required]
        public required string Title { get; set; }


        [Column(TypeName = "VARCHAR(MAX)")]
        [Required]
        public string Content { get; set; } = null!;

        public Guid SectionId { get; set; }

        public Section? Section { get; set; }
    }
}
