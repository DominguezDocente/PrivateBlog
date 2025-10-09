using PrivateBlog.Web.Data.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrivateBlog.Web.Data.Entities
{
    public class Blog : IId
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(64)]
        public required string Name { get; set; }

        [Column(TypeName = "VARCHAR(MAX)")]
        public string Content { get; set; } = null!;

        public required Guid SectionId { get; set; }

        public Section? Section { get; set; }
    }
}
