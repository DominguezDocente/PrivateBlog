using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
        public async Task GetPagination_ReturnsAllSections()
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
        public async Task GetPagination_ReturnsTwoPages()
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

        [TestMethod]
        public async Task GetOne_ReturnsNotFoud()
        {
            // Arrange
            string dbName = Guid.NewGuid().ToString();
            DataContext context = BuildContext(dbName);
            IMapper mapper = ConfigureAutoMapper();

            // Act
            ISectionsService service = new SectionsService(context, mapper);

            Response<SectionDTO> response = await service.GetOneAsync(1);

            // Assert
            Assert.IsTrue(!response.IsSuccess); 
            Assert.IsTrue(response.Message.StartsWith("No existe registro con id"));
        }

        [TestMethod]
        public async Task GetOne_ReturnsSection()
        {
            // Arrange
            string dbName = Guid.NewGuid().ToString();
            DataContext context = BuildContext(dbName);
            IMapper mapper = ConfigureAutoMapper();

            Section section = new Section { Name = "Section A" };

            context.Sections.Add(section);

            await context.SaveChangesAsync();

            // Act
            DataContext context2 = BuildContext(dbName);
            ISectionsService service = new SectionsService(context2, mapper);

            Response<SectionDTO> response = await service.GetOneAsync(section.Id);

            // Assert
            Assert.IsTrue(response.IsSuccess);
            Assert.AreEqual(response.Result.Name, section.Name);
        }

        [TestMethod]
        public async Task Create_ReturnsSuccess()
        {
            // Arrange
            string dbName = Guid.NewGuid().ToString();
            DataContext context = BuildContext(dbName);
            IMapper mapper = ConfigureAutoMapper();

            SectionDTO dto = new SectionDTO { Name = "Section A" };

            // Act
            ISectionsService service = new SectionsService(context, mapper);
            Response<SectionDTO> response = await service.CreateAsync(dto);

            // Assert
            Assert.IsTrue(response.IsSuccess);

            DataContext context2 = BuildContext(dbName);
            int quantity = await context2.Sections.CountAsync();
            Assert.AreEqual(1, quantity);

            Section section = await context2.Sections.FirstOrDefaultAsync();
            Assert.AreEqual(section.Name, dto.Name);
        }
    }
}
