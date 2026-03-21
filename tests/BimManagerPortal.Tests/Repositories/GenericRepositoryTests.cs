using BimManagerPortal.Domain.Entities.BigDataPlugins;
using BimManagerPortal.Persistance.Contexts;
using BimManagerPortal.Persistance.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BimManagerPortal.Tests.Repositories;

public class GenericRepositoryTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly GenericRepository<PluginBigData> _sut;

    public GenericRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _sut = new GenericRepository<PluginBigData>(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldPersistEntity()
    {
        var entity = new PluginBigData("user", "TestPlugin", [1, 2, 3]);

        await _sut.AddAsync(entity);
        await _context.SaveChangesAsync();

        var stored = await _context.BigDataPlugins.FindAsync(entity.Id);
        stored.Should().NotBeNull();
        stored!.PluginName.Should().Be("TestPlugin");
    }

    [Fact]
    public async Task GetByIdAsync_WhenEntityExists_ShouldReturnIt()
    {
        var entity = new PluginBigData("user", "Plugin", [0xFF]);
        await _sut.AddAsync(entity);
        await _context.SaveChangesAsync();

        var result = await _sut.GetByIdAsync(entity.Id.ToString());

        result.Should().NotBeNull();
        result!.Id.Should().Be(entity.Id);
    }

    [Fact]
    public async Task GetByIdAsync_WhenNotFound_ShouldReturnNull()
    {
        var result = await _sut.GetByIdAsync(Guid.NewGuid().ToString());

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetByIdAsync_WhenIdIsNotGuid_ShouldReturnDefault()
    {
        var result = await _sut.GetByIdAsync("not-a-guid");

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllEntities()
    {
        await _sut.AddAsync(new PluginBigData("u1", "P1", [1]));
        await _sut.AddAsync(new PluginBigData("u2", "P2", [2]));
        await _context.SaveChangesAsync();

        var result = await _sut.GetAllAsync();

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveEntity()
    {
        var entity = new PluginBigData("user", "ToDelete", [1]);
        await _sut.AddAsync(entity);
        await _context.SaveChangesAsync();

        await _sut.DeleteAsync(entity);
        await _context.SaveChangesAsync();

        var result = await _context.BigDataPlugins.FindAsync(entity.Id);
        result.Should().BeNull();
    }

    [Fact]
    public async Task Entities_ShouldExposeQueryableSet()
    {
        await _sut.AddAsync(new PluginBigData("u1", "Alpha", [1]));
        await _sut.AddAsync(new PluginBigData("u2", "Beta", [2]));
        await _context.SaveChangesAsync();

        var count = await _sut.Entities.CountAsync();

        count.Should().Be(2);
    }

    [Fact]
    public async Task UpdateAsync_ShouldModifyExistingEntity()
    {
        var entity = new PluginBigData("user", "OldName", [1]);
        await _sut.AddAsync(entity);
        await _context.SaveChangesAsync();

        entity.PluginName = "NewName";
        await _sut.UpdateAsync(entity);
        await _context.SaveChangesAsync();

        var updated = await _context.BigDataPlugins.FindAsync(entity.Id);
        updated!.PluginName.Should().Be("NewName");
    }

    public void Dispose() => _context.Dispose();
}
