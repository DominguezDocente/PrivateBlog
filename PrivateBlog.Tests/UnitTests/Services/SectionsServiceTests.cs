using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Web.Controllers;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Data;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.DTOs;
using PrivateBlog.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateBlog.Tests.UnitTests.Services
{
    [TestClass]
    public class SectionsServiceTests : BaseTests
    {
        [TestMethod]
        public async Task GetListAsync_ReturnAllSections()
        {
            // Arrange
            string nameDb = Guid.NewGuid().ToString();
            DataContext context = BuildContext(nameDb);
            IMapper mapper = ConfigureAutoMapper();

            context.Sections.AddRange(new List<Section>
            {
                new Section { Name = "Sección A" },
                new Section { Name = "Sección B" },
                new Section { Name = "Sección C" },
            });

            await context.SaveChangesAsync();

            // Act
            DataContext context2 = BuildContext(nameDb);
            ISectionsService service = new SectionsService(context2, mapper);

            Response<List<SectionDTO>> response = await service.GetListAsync();

            // Assert
            Assert.IsTrue(response.IsSuccess);
            Assert.AreEqual(3, response.Result.Count);
        }
    }
}
