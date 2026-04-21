using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Persistence.Seeding
{
    internal interface ISeedable
    {
        Task SeedAsync();
    }
}
