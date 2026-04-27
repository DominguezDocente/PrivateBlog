using PrivateBlog.Domain.Entities.Sections;
using PrivateBlog.Domain.Exceptions;

namespace PrivateBlog.Domain.Entities.Blogs
{
    public class Blog
    {
        public Guid Id { get; private set; }

        /// <summary>Título del artículo.</summary>
        public string Name { get; private set; } = null!;

        /// <summary>Cuerpo en HTML.</summary>
        public string Content { get; private set; } = null!;

        public Guid SectionId { get; private set; }

        public Section Section { get; private set; } = null!;

        /// <summary>Auditoría de creación (UTC).</summary>
        public DateTime CreatedAtUtc { get; private set; }

        /// <summary>Última modificación (UTC).</summary>
        public DateTime UpdatedAtUtc { get; private set; }

        /// <summary>Borrador vs publicado.</summary>
        public bool IsPublished { get; private set; }

        private Blog()
        {
        }

        public Blog(string name, string content, Guid sectionId, bool isPublished = false)
        {
            ApplyNameRules(name);

            Name = name.Trim();
            Content = content ?? string.Empty;
            SectionId = sectionId;
            IsPublished = isPublished;

            Id = Guid.CreateVersion7();

            DateTime now = DateTime.UtcNow;
            CreatedAtUtc = now;
            UpdatedAtUtc = now;
        }

        public void Update(string name, string content, Guid sectionId, bool isPublished)
        {
            ApplyNameRules(name);

            Name = name.Trim();
            Content = content ?? string.Empty;
            SectionId = sectionId;
            IsPublished = isPublished;
            UpdatedAtUtc = DateTime.UtcNow;
        }

        public void Publish()
        {
            IsPublished = true;
            UpdatedAtUtc = DateTime.UtcNow;
        }

        public void Unpublish()
        {
            IsPublished = false;
            UpdatedAtUtc = DateTime.UtcNow;
        }

        private static void ApplyNameRules(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new BusinessRuleException($"El {nameof(name)} es requerido.");
            }

            string trimmed = name.Trim();

            if (trimmed.Length < 3)
            {
                throw new BusinessRuleException($"El {nameof(name)} debe tener al menos 3 caracteres.");
            }

            if (trimmed.Length > 200)
            {
                throw new BusinessRuleException($"El {nameof(name)} debe tener como máximo 200 caracteres.");
            }
        }
    }
}
