using PrivateBlog.Domain.Entities.Sections;
using PrivateBlog.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Tests.UnitTests.Domain.Entities.Sections
{
    [TestClass]
    public class SectionTests
    {
        [TestMethod]
        public void Constructor_WithValidName_CreatesActiveSecttion() 
        {
            // Arrange
            string name = "Tecnología";

            // Act
            Section section = new Section(name);

            // Assert
            Assert.AreNotEqual(Guid.Empty, section.Id);
            Assert.AreEqual(name, section.Name);
            Assert.IsTrue(section.IsActive);
        }

        [TestMethod]
        public void Constructor_WithWhitespaceName_ThrowsBussinesRuleException()
        {
            // Arrange
            string name = "    ";

            // Act & Assert
            Assert.ThrowsExactly<BussinesRuleException>(() => new Section(name));
        }

        [TestMethod]
        public void Constructor_WithShorterName_ThrowsBussinesRuleException()
        {
            // Arrange
            string name = "abc";

            // Act & Assert
            Assert.ThrowsExactly<BussinesRuleException>(() => new Section(name));
        }

        [TestMethod]
        public void Constructor_WithLargerName_ThrowsBussinesRuleException()
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
            Section section = new Section("General");
            string newName = "Tecnología Actualizada";

            // Act
            section.UpdateName(newName);

            // Assert
            Assert.AreEqual(newName, section.Name);
        }

        [TestMethod]
        public void UpdateName_WithInvalidName_ThrowsBussinesRuleException()
        {
            // Arrange
            Section section = new Section("General");
            string newInvalidName = "abc";

            // Act & Assert
            Assert.ThrowsExactly<BussinesRuleException>(() => section.UpdateName(newInvalidName));
        }

        [TestMethod]
        public void Deactivate_SetsIsActiveToFalse()
        {
            // Arrange
            Section section = new Section("General");

            // Act
            section.Deactivate();

            // Assert
            Assert.IsFalse(section.IsActive);
        }

        [TestMethod]
        public void Activate_AfterDeactivate_SetsIsActiveToTrue()
        {
            // Arrange
            Section section = new Section("General");
            section.Deactivate();

            // Act
            section.Activate();

            // Assert
            Assert.IsTrue(section.IsActive);
        }
    }
}
