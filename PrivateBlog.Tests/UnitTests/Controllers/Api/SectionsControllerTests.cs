using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PrivateBlog.Web.Controllers.Api;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Core.Pagination;
using PrivateBlog.Web.DTOs;
using PrivateBlog.Web.Helpers;
using PrivateBlog.Web.Services;

namespace PrivateBlog.Tests.UnitTests.Controllers.Api
{
    [TestClass]
    public class SectionsControllerTests : BaseTests
    {
        [TestMethod]
        public async Task GetSections_ReturnsStatus200()
        {
            // Arrange
            Response<PaginationResponse<SectionDTO>> mockResponse = new() 
            {
                IsSuccess = true,
                Result = new PaginationResponse<SectionDTO>(),
            };

            Mock<ISectionsService> sectionsServiceMock = new Mock<ISectionsService>();
            sectionsServiceMock.Setup(x => x.GetPaginationAsync(It.IsAny<PaginationRequest>()))
                               .Returns(Task.FromResult(mockResponse));

            // Act
            SectionsController controller = new SectionsController(sectionsServiceMock.Object);
            IActionResult actionResult = await controller.GetSections(new PaginationRequest());

            // Assert
            ObjectResult result = actionResult as ObjectResult;
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
        }

        [TestMethod]
        public async Task GetSections_ReturnsStatus400()
        {
            // Arrange
            Response<PaginationResponse<SectionDTO>> mockResponse = new()
            {
                IsSuccess = false,
                Result = null,
            };

            Mock<ISectionsService> sectionsServiceMock = new Mock<ISectionsService>();
            sectionsServiceMock.Setup(x => x.GetPaginationAsync(It.IsAny<PaginationRequest>()))
                               .Returns(Task.FromResult(mockResponse));

            // Act
            SectionsController controller = new SectionsController(sectionsServiceMock.Object);
            IActionResult actionResult = await controller.GetSections(new PaginationRequest());

            // Assert
            ObjectResult result = actionResult as ObjectResult;
            Assert.AreEqual(StatusCodes.Status400BadRequest, result.StatusCode);
        }
    }
}
