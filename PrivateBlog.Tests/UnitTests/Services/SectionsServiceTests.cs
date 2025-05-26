using AutoMapper;
using PrivateBlog.Web.Core;
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

namespace PrivateBlog.Tests.UnitTests.Services
{
    [TestClass]
    public class SectionsServiceTests : BaseTests
    {
        [TestMethod]
        public async Task GetPagination_ReturnAllSections()
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
            ISectionsService service = new SectionsService(context2, mapper);

            Response<PaginationResponse<SectionDTO>> response = await service.GetPaginationAsync(new PaginationRequest());

            // Assert
            Assert.IsTrue(response.IsSuccess);
            Assert.AreEqual(3, response.Result.List.Count);
        }

        [TestMethod]
        public async Task GetPagination_ReturnTwoPages()
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
            ISectionsService service = new SectionsService(context2, mapper);

            PaginationRequest request = new PaginationRequest
            {
                Page = 1,
                RecordsPerPage = 2
            };

            Response<PaginationResponse<SectionDTO>> response = await service.GetPaginationAsync(request);

            // Assert
            Assert.IsTrue(response.IsSuccess);
            Assert.AreEqual(2, response.Result.Pages.Count);
            Assert.AreEqual(2, response.Result.List.Count);
        }
    }
}