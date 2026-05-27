using PrivateBlog.Domain.Entities.Sections;
using PrivateBlog.Domain.Exceptions;

namespace PrivateBlog.Tests.UnitTests.Domain.Entities;

[TestClass]
public sealed class SectionTests
{
    [TestMethod]
    public void Constructor_WithValidName_CreatesActiveSection()
    {
        // Arrange
        const string name = "Tecnología";

        // Act
        Section section = new Section(name);

        // Assert
        Assert.AreNotEqual(Guid.Empty, section.Id);
        Assert.AreEqual(name, section.Name);
        Assert.IsTrue(section.IsActive);
    }

    [TestMethod]
    public void Constructor_WithEmptyName_ThrowsBussinesRuleException()
    {
        // Arrange
        const string name = "";

        // Act & Assert
        Assert.ThrowsExactly<BussinesRuleException>(() => new Section(name));
    }

    [TestMethod]
    public void Constructor_WithWhitespaceName_ThrowsBussinesRuleException()
    {
        // Arrange
        const string name = "   ";

        // Act & Assert
        Assert.ThrowsExactly<BussinesRuleException>(() => new Section(name));
    }

    [TestMethod]
    public void Constructor_WithNameShorterThanFourCharacters_ThrowsBussinesRuleException()
    {
        // Arrange
        const string name = "abc";

        // Act & Assert
        Assert.ThrowsExactly<BussinesRuleException>(() => new Section(name));
    }

    [TestMethod]
    public void Constructor_WithNameLongerThanSixtyFourCharacters_ThrowsBussinesRuleException()
    {
        // Arrange
        string name = new string('a', 65);

        // Act & Assert
        Assert.ThrowsExactly<BussinesRuleException>(() => new Section(name));
    }

    [TestMethod]
    public void UpdateName_WithValidName_UpdatesName()
    {
        // Arrange
        Section section = new Section("Sección inicial");
        const string newName = "Sección actualizada";

        // Act
        section.UpdateName(newName);

        // Assert
        Assert.AreEqual(newName, section.Name);
    }

    [TestMethod]
    public void UpdateName_WithInvalidName_ThrowsBussinesRuleException()
    {
        // Arrange
        Section section = new Section("Sección inicial");

        // Act & Assert
        Assert.ThrowsExactly<BussinesRuleException>(() => section.UpdateName("abc"));
    }

    [TestMethod]
    public void Deactivate_SetsIsActiveToFalse()
    {
        // Arrange
        Section section = new Section("Sección activa");

        // Act
        section.Deactivate();

        // Assert
        Assert.IsFalse(section.IsActive);
    }

    [TestMethod]
    public void Activate_AfterDeactivate_SetsIsActiveToTrue()
    {
        // Arrange
        Section section = new Section("Sección activa");
        section.Deactivate();

        // Act
        section.Activate();

        // Assert
        Assert.IsTrue(section.IsActive);
    }
}
