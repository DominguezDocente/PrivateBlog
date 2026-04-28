using PrivateBlog.Domain.Entities.Sections;
using PrivateBlog.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Domain.Entities.Blogs
{
    public sealed class Blog
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;
        public string Content { get; private set; } = null!;
        public Guid SectionId { get; private set; }
        public Section Section { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public bool IsPublished { get; private set; }

        private Blog()
        {            
        }

        public Blog(string name, string content, Guid sectionId, bool isPublished = false)
        {
            ApplyNameRules(name);

            Name = name;
            Content = content;
            SectionId = sectionId;
            CreatedAt = DateTime.UtcNow;
            IsPublished = isPublished;
            Id = Guid.CreateVersion7();
        }

        public void Publish()
        {
            IsPublished = true;
        }

        public void Unpublish()
        {
            IsPublished = false;
        }

        public void Update(string name, string content, Guid sectionId, bool isPublished = false)
        {
            ApplyNameRules(name);
            ApplyContentRules(content);

            Name = name;
            Content = content;
            SectionId = sectionId;
            CreatedAt = DateTime.UtcNow;
            IsPublished = isPublished;
        }


        private static void ApplyNameRules(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new BussinesRuleException("El nombre es requerido..");
            }

            if (name.Length > 128)
            {
                throw new BussinesRuleException("El nombre no puede exceder 128 caracteres.");
            }

            if (name.Length < 3)
            {
                throw new BussinesRuleException("El nombre debe tener al menos 3 caracteres.");
            }
        }

        private static void ApplyContentRules(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new BussinesRuleException("El contenido es requerido..");
            }

            if (content.Length < 8)
            {
                throw new BussinesRuleException("El contenido debe tener al menos 8 caracteres.");
            }
        }
    }
}
