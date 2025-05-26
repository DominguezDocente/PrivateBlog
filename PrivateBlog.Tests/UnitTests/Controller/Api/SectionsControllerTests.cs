using AutoMapper;
using Moq;
using PrivateBlog.Web.Controllers.Api;
using PrivateBlog.Web.Data;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateBlog.Tests.UnitTests.Controller.Api
{
    [TestClass]
    public class SectionsControllerTests : BaseTests
    {
        public async Task TESTESTE()
        {
            // Arrange
            string dbName = Guid.NewGuid().ToString();
            DataContext context = BuildContext(dbName);
            IMapper mapper = ConfigureAutoMapper();


            context.AddRange(new List<Section>
            {
                new Section { Name = "Sección A" },
                new Section { Name = "Sección B" },
                new Section { Name = "Sección C" },
            });

            await context.SaveChangesAsync();

            // Act
            DataContext context2 = BuildContext(dbName);

            Mock<ISectionsService> sectionsServiceMock = new Mock<ISectionsService>();

            SectionsController controller = new SectionsController(sectionsServiceMock.Object);

        }

    }
}
