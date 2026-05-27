using Moq;
using PrivateBlog.Application.Contracts.Persisntece;
using PrivateBlog.Application.Contracts.Repositories;
using PrivateBlog.Application.UseCases.Sections.Commands.CreateSection;
using PrivateBlog.Domain.Entities.Sections;
using PrivateBlog.Domain.Exceptions;

namespace PrivateBlog.Tests.UnitTests.Application.UseCases.Sections.Commands.CreateSection;

[TestClass]
public sealed class CreateSectionUseCaseTests
{
    private Mock<ISectionsRepository> _sectionsRepositoryMock = null!;
    private Mock<IUnitOfWork> _unitOfWorkMock = null!;
    private CreateSectionUseCase _useCase = null!;

    [TestInitialize]
    public void SetUp()
    {
        // Arrange (fixture común)
        _sectionsRepositoryMock = new Mock<ISectionsRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _useCase = new CreateSectionUseCase(_sectionsRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [TestMethod]
    public async Task Handle_WithValidCommand_CreatesSectionCommitsAndReturnsId()
    {
        // Arrange
        const string sectionName = "Noticias";
        CreateSectionCommand command = new CreateSectionCommand { Name = sectionName };
        Section? capturedSection = null;

        _sectionsRepositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<Section>()))
            .Callback<Section>(s => capturedSection = s)
            .ReturnsAsync((Section s) => s);

        _unitOfWorkMock
            .Setup(u => u.CommitAsync())
            .Returns(Task.CompletedTask);

        // Act
        Guid resultId = await _useCase.Handle(command);

        // Assert
        Assert.IsNotNull(capturedSection);
        Assert.AreEqual(sectionName, capturedSection!.Name);
        Assert.AreEqual(resultId, capturedSection.Id);
        Assert.AreNotEqual(Guid.Empty, resultId);

        _sectionsRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<Section>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
        _unitOfWorkMock.Verify(u => u.RollbackAsync(), Times.Never);
    }

    [TestMethod]
    public async Task Handle_WhenRepositoryThrows_RollsBackAndRethrows()
    {
        // Arrange
        CreateSectionCommand command = new CreateSectionCommand { Name = "Noticias" };

        _sectionsRepositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<Section>()))
            .ThrowsAsync(new InvalidOperationException("Error al guardar la sección."));

        _unitOfWorkMock
            .Setup(u => u.RollbackAsync())
            .Returns(Task.CompletedTask);

        // Act & Assert
        await Assert.ThrowsExactlyAsync<InvalidOperationException>(() => _useCase.Handle(command));

        _unitOfWorkMock.Verify(u => u.RollbackAsync(), Times.Once);
        _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Never);
    }

    [TestMethod]
    public async Task Handle_WhenCommitThrows_RollsBackAndRethrows()
    {
        // Arrange
        CreateSectionCommand command = new CreateSectionCommand { Name = "Noticias" };

        _sectionsRepositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<Section>()))
            .ReturnsAsync((Section s) => s);

        _unitOfWorkMock
            .Setup(u => u.CommitAsync())
            .ThrowsAsync(new InvalidOperationException("Error al confirmar la transacción."));

        _unitOfWorkMock
            .Setup(u => u.RollbackAsync())
            .Returns(Task.CompletedTask);

        // Act & Assert
        await Assert.ThrowsExactlyAsync<InvalidOperationException>(() => _useCase.Handle(command));

        _sectionsRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<Section>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.RollbackAsync(), Times.Once);
    }

    [TestMethod]
    public async Task Handle_WithInvalidSectionName_ThrowsBeforeRepositoryIsCalled()
    {
        // Arrange
        CreateSectionCommand command = new CreateSectionCommand { Name = "abc" };

        // Act & Assert
        await Assert.ThrowsExactlyAsync<BussinesRuleException>(() => _useCase.Handle(command));

        _sectionsRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<Section>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Never);
        _unitOfWorkMock.Verify(u => u.RollbackAsync(), Times.Never);
    }
}
