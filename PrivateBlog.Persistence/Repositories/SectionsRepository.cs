using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Domain.Entities.Sections;

namespace PrivateBlog.Persistence.Repositories
{
    public class SectionsRepository : Repository<Section>, ISectionsRepository
    {
        public SectionsRepository(DataContext context) : base(context)
        {
        }
    }
}
