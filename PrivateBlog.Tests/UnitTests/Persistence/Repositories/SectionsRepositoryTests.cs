using PrivateBlog.Application.Contracts.Pagination;
using PrivateBlog.Domain.Entities.Blogs;
using PrivateBlog.Domain.Entities.Sections;
using PrivateBlog.Persistence;
using PrivateBlog.Persistence.Repositories;

namespace PrivateBlog.Tests.UnitTests.Persistence.Repositories;

[TestClass]
public sealed class SectionsRepositoryTests : BaseTests
{
    private DataContext _context = null!;
    private SectionsRepository _repository = null!;

    [TestInitialize]
    public void SetUp()
    {
        _context = BuildContext();
        _repository = new SectionsRepository(_context);
    }

    [TestCleanup]
    public void TearDown()
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

        // Assert
        Section? stored = await _context.Sections.FindAsync(section.Id);
        Assert.IsNotNull(stored);
        Assert.AreEqual("Noticias", stored!.Name);
        Assert.IsTrue(stored.IsActive);
    }

    [TestMethod]
    public async Task GetByIdAsync_WhenSectionExists_ReturnsSection()
    {
        // Arrange
        Section section = new Section("Deportes");
        _context.Sections.Add(section);
        await SaveChangesAsync(_context);

        // Act
        Section? result = await _repository.GetByIdAsync(section.Id);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(section.Id, result!.Id);
        Assert.AreEqual("Deportes", result.Name);
    }

    [TestMethod]
    public async Task GetByIdAsync_WhenSectionDoesNotExist_ReturnsNull()
    {
        // Arrange
        Guid missingId = Guid.CreateVersion7();

        // Act
        Section? result = await _repository.GetByIdAsync(missingId);

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public async Task GetPagedList_WithoutFilters_ReturnsAllSectionsOrderedByName()
    {
        // Arrange
        await SeedSectionsAsync();

        PaginationRequest pagination = new PaginationRequest(1, 10);

        // Act
        (List<Section> items, int totalCount) = await _repository.GetPagedList(pagination, null, null);

        // Assert
        Assert.AreEqual(3, totalCount);
        Assert.AreEqual(3, items.Count);
        Assert.AreEqual("Alpha Team", items[0].Name);
        Assert.AreEqual("Beta News", items[1].Name);
        Assert.AreEqual("Gamma Zone", items[2].Name);
    }

    [TestMethod]
    public async Task GetPagedList_WithNameFilter_ReturnsMatchingSections()
    {
        // Arrange
        await SeedSectionsAsync();

        PaginationRequest pagination = new PaginationRequest(1, 10);

        // Act
        (List<Section> items, int totalCount) = await _repository.GetPagedList(pagination, "Beta", null);

        // Assert
        Assert.AreEqual(1, totalCount);
        Assert.AreEqual(1, items.Count);
        Assert.AreEqual("Beta News", items[0].Name);
    }

    [TestMethod]
    public async Task GetPagedList_WithActiveFilter_ReturnsOnlyActiveSections()
    {
        // Arrange
        await SeedSectionsAsync();

        PaginationRequest pagination = new PaginationRequest(1, 10);

        // Act
        (List<Section> items, int totalCount) = await _repository.GetPagedList(pagination, null, isActiveFilter: true);

        // Assert
        Assert.AreEqual(2, totalCount);
        Assert.IsTrue(items.All(s => s.IsActive));
    }

    [TestMethod]
    public async Task GetPagedList_WithPagination_ReturnsRequestedPage()
    {
        // Arrange
        await SeedSectionsAsync();

        PaginationRequest pagination = new PaginationRequest(2, 2);

        // Act
        (List<Section> items, int totalCount) = await _repository.GetPagedList(pagination, null, null);

        // Assert
        Assert.AreEqual(3, totalCount);
        Assert.AreEqual(1, items.Count);
        Assert.AreEqual("Gamma Zone", items[0].Name);
    }

    [TestMethod]
    public async Task HasArticlesAsync_WhenSectionHasBlogs_ReturnsTrue()
    {
        // Arrange
        Section section = new Section("Tecnología");
        Blog blog = new Blog("Primer post", "Contenido del blog", section.Id, isPublished: true);

        _context.Sections.Add(section);
        _context.Blogs.Add(blog);
        await SaveChangesAsync(_context);

        // Act
        bool result = await _repository.HasArticlesAsync(section.Id);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task HasArticlesAsync_WhenSectionHasNoBlogs_ReturnsFalse()
    {
        // Arrange
        Section section = new Section("Cultura");
        _context.Sections.Add(section);
        await SaveChangesAsync(_context);

        // Act
        bool result = await _repository.HasArticlesAsync(section.Id);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task DeleteAsync_RemovesSectionFromContext()
    {
        // Arrange
        Section section = new Section("Temporal");
        _context.Sections.Add(section);
        await SaveChangesAsync(_context);

        // Act
        await _repository.DeleteAsync(section);
        await SaveChangesAsync(_context);

        // Assert
        Section? stored = await _context.Sections.FindAsync(section.Id);
        Assert.IsNull(stored);
    }

    private async Task SeedSectionsAsync()
    {
        Section alpha = new Section("Alpha Team");
        Section beta = new Section("Beta News");
        Section gamma = new Section("Gamma Zone");
        gamma.Deactivate();

        _context.Sections.AddRange(alpha, beta, gamma);
        await SaveChangesAsync(_context);
    }
}
