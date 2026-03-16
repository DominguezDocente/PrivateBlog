using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Domain.Entities.Sections;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Persistence.Repositories
{
    public class SectionsRepository : Repository<Section>, ISectionsRepository
    {
        public SectionsRepository(DataContext context) : base(context)
        {
        }
    }
}
