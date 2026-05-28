using Moq;
using PrivateBlog.Application.Contracts.Persisntece;
using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.UseCases.Sections.Commands.CreateSection;
using PrivateBlog.Domain.Entities.Sections;
using PrivateBlog.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Tests.UnitTests.Application.UseCases.Sections.Commands
{
    [TestClass]
    public class CreateSectionUseCaseTests
    {
        private Mock<ISectionsRepository> _repository;
        private Mock<IUnitOfWork> _unitOfWork;
        private CreateSectionUseCase _useCase;

        [TestInitialize]
        public void Setup()
        {
            _repository = new Mock<ISectionsRepository>();
            _unitOfWork = new Mock<IUnitOfWork>();
            _useCase = new CreateSectionUseCase(_repository.Object, _unitOfWork.Object);
        }

        [TestMethod]
        public async Task Handle_WithValidCommand_CreatesSectionCommitsAndReturnsId() 
        {
            // Arrange
            string sectionName = "Noticias";
            CreateSectionCommand command = new CreateSectionCommand { Name = sectionName };
            Section? capturedSection = null;

            _repository.Setup(r => r.CreateAsync(It.IsAny<Section>()))
                                    .Callback<Section>(s => capturedSection = s)
                                    .ReturnsAsync((Section s) => s);

            _unitOfWork.Setup(u => u.CommitAsync())
                       .Returns(Task.CompletedTask);

            // Act
            Guid resultId = await _useCase.Handle(command);

            // Assert
            Assert.IsNotNull(capturedSection);
            Assert.AreEqual(sectionName, capturedSection.Name);
            Assert.AreEqual(resultId, capturedSection.Id);
            Assert.AreNotEqual(Guid.Empty, resultId);

            _repository.Verify(r => r.CreateAsync(It.IsAny<Section>()), Times.Once);
            _unitOfWork.Verify(u => u.CommitAsync(), Times.Once);
            _unitOfWork.Verify(u => u.RollbackAsync(), Times.Never);
        }

        [TestMethod]
        public async Task Handle_WhenRepositoryThrowsException_RollsbackAndRethrows()
        {
            // Arrange
            CreateSectionCommand command = new CreateSectionCommand { Name = "test" };

            _repository.Setup(r => r.CreateAsync(It.IsAny<Section>()))
                                    .ThrowsAsync(new InvalidOperationException("Error al crear la sección"));

            _unitOfWork.Setup(u => u.RollbackAsync())
                       .Returns(Task.CompletedTask);

            // Act & Assert
            await Assert.ThrowsExactlyAsync<InvalidOperationException>(async () => await _useCase.Handle(command));

            _unitOfWork.Verify(u => u.CommitAsync(), Times.Never);
            _unitOfWork.Verify(u => u.RollbackAsync(), Times.Once);
        }

        [TestMethod]
        public async Task Handle_WhenCommitThrowsException_RollsbackAndRethrows()
        {
            // Arrange
            CreateSectionCommand command = new CreateSectionCommand { Name = "test" };

            _repository.Setup(r => r.CreateAsync(It.IsAny<Section>()))
                                    .ReturnsAsync((Section s) => s);

            _unitOfWork.Setup(u => u.CommitAsync())
                       .ThrowsAsync(new BussinesRuleException("Error al intentar almacenar la sección"));

            _unitOfWork.Setup(u => u.RollbackAsync())
                       .Returns(Task.CompletedTask);

            // Act & Assert
            await Assert.ThrowsExactlyAsync<BussinesRuleException>(async () => await _useCase.Handle(command));

            _repository.Verify(u => u.CreateAsync(It.IsAny<Section>()), Times.Once);
            _unitOfWork.Verify(u => u.CommitAsync(), Times.Once);
            _unitOfWork.Verify(u => u.RollbackAsync(), Times.Once);
        }
    }
}
