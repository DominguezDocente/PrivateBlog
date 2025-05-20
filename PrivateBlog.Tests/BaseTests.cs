using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateBlog.Tests
{
    public class BaseTests
    {
        protected DataContext BuildContext(string dbName)
        {
            DbContextOptions<DataContext> options = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase(dbName).Options;

            DataContext dataContext = new DataContext(options);

            return dataContext;
        }

        protected IMapper ConfigureAutoMapper()
        {
            MapperConfiguration config = new MapperConfiguration(options =>
            {
                options.AddProfile(new AutoMapperProfiles());
            });

            return config.CreateMapper();
        }
    }
}
