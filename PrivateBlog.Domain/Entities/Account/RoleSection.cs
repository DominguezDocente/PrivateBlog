using PrivateBlog.Domain.Entities.Sections;
using PrivateBlog.Domain.Exceptions;

namespace PrivateBlog.Domain.Entities.Account
{
    public class RoleSection
    {
        public Guid RoleId { get; private set; }
        public Guid SectionId { get; private set; }
        public Role Role { get; private set; }
        public Section Section { get; private set; }

        private RoleSection()
        {
        }

        public RoleSection(Guid roleId, Guid sectionId)
        {
            if (roleId == Guid.Empty)
            {
                throw new BussinesRuleException("El Id del rol es requerido.");
            }

            if (sectionId == Guid.Empty)
            {
                throw new BussinesRuleException("El Id de la sección es requerido.");
            }

            RoleId = roleId;
            SectionId = sectionId;
        }
    }
}
