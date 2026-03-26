using PrivateBlog.Domain.Exceptions;

namespace PrivateBlog.Domain.Entities.Sections
{
    public sealed class Section
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;
        public bool IsActive { get; private set; }

        private Section()
        {            
        }

        public Section(string name)
        {
            ApplyBussinesRulesForName(name);
            Id = Guid.CreateVersion7();
            Name = name;
            IsActive = true;
        }

        public void UpdateName(string name)
        {
            ApplyBussinesRulesForName(name);
            Name = name;
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        private void ApplyBussinesRulesForName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new BussinesRuleException($"El {nameof(name)} es requerido.");
            }

            if (name.Length < 4)
            {
                throw new BussinesRuleException($"El {nameof(name)} debe ser mayor a 4 letras.");
            }

            if (name.Length > 64)
            {
                throw new BussinesRuleException($"El {nameof(name)} debe ser menor a 64 letras.");
            }
        }
    }
}