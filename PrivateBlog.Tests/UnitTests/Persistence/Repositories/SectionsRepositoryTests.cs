using PrivateBlog.Domain.Entities.Sections;
using PrivateBlog.Persistence;
using PrivateBlog.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Tests.UnitTests.Persistence.Repositories
{
    [TestClass]
    public class SectionsRepositoryTests : BaseTests
    {
        private DataContext _context;
        private SectionsRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            _context = BuildContext();
            _repository = new SectionsRepository(_context);
        }

        [TestCleanup] 
        public void Cleanup() 
        {
            _context.Dispose();
        }

        [TestMethod]
        public async Task CreateAsync_WithValidSection_PersistsEntityAfterSaveChanges() 
        {
            // Arrange
            Section section = new Section("Noticias");

            // Act
            await _repository.CreateAsync(section);
            await SaveChangesAsync(_context);

            Section? persistedSection = await _context.Sections.FindAsync(section.Id);

            // Assert
            Assert.IsNotNull(persistedSection);
            Assert.AreEqual("Noticias", persistedSection.Name);
            Assert.IsTrue(persistedSection.IsActive);
        }

        [TestMethod]
        public async Task GetByIdAsync_WhenSectionExists_ReturnsSection()
        {
            // Arrange
            Section section = new Section("Noticias");
            _context.Sections.Add(section);
            await SaveChangesAsync(_context);

            // Act
            Section? result = await _repository.GetByIdAsync(section.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(section.Id, result.Id);
            Assert.AreEqual("Noticias", result.Name);
        }

        [TestMethod]
        public async Task GetByIdAsync_WhenSectionDoesNotExists_ReturnsNull()
        {
            // Arrange
            Guid missingId = Guid.CreateVersion7();

            // Act
            Section? result = await _repository.GetByIdAsync(missingId);

            // Assert
            Assert.IsNull(result);
        }
    }
}
