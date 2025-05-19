using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PrivateBlog.Web.Controllers;
using PrivateBlog.Web.Core.Pagination;
using PrivateBlog.Web.Data;
using PrivateBlog.Web.Data.Entities;
using PrivateBlog.Web.DTOs;
using PrivateBlog.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateBlog.Tests.UnitTests.Controller
{
    [TestClass]
    public class SectionsControllerTests : BaseTests
    {

        //[TestMethod]
        public async Task Index_ReturnAllSections()
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
            SectionsController controller = new SectionsController(service, null);

            ViewResult result = await controller.Index(new PaginationRequest()) as ViewResult;

            //// Assert
            //List<SectionDTO> sections = (List<SectionDTO>)result.Model;
            //Assert.AreEqual(3, sections.Count);
        }

    }
}
