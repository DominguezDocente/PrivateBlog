using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Domain.Entities.Sections;

namespace PrivateBlog.Persistence.Respositories
{
    public class SectionsRepository : Repository<Section>, ISectionsRepository
    {
        public SectionsRepository(DataContext context) : base(context)
        {
        }
    }
}
