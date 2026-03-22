using System.Text;
using BimManagerPortal.Application.Features.PluginBigDatas.Queries.GetPluginBigDatas;
using BimManagerPortal.Application.Interfaces.Compress;
using BimManagerPortal.Application.Interfaces.Repositories;
using BimManagerPortal.Domain.Entities.BigDataPlugins;
using BimManagerPortal.Tests.Helpers;

namespace BimManagerPortal.Tests.Handlers;

public class GetPluginBigDatasQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ICompressionService> _compressionMock = new();
    private readonly Mock<IGenericRepository<PluginBigData>> _repoMock = new();

    private readonly GetPluginBigDatasQueryHandler _sut;

    public GetPluginBigDatasQueryHandlerTests()
    {
        _unitOfWorkMock
            .Setup(u => u.Repository<PluginBigData>())
            .Returns(_repoMock.Object);

        _sut = new GetPluginBigDatasQueryHandler(
            _unitOfWorkMock.Object,
            _compressionMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnDtoForEachEntity()
    {
        var jsonBytes = Encoding.UTF8.GetBytes("{\"v\":1}");

        var entities = new List<PluginBigData>
        {
            new("user1", "PluginA", jsonBytes),
            new("user2", "PluginB", jsonBytes),
        };

        _repoMock
            .Setup(r => r.Entities)
            .Returns(entities.AsAsyncQueryable());

        _compressionMock
            .Setup(c => c.Decompress(jsonBytes))
            .Returns(jsonBytes);

        var result = await _sut.Handle(new GetPluginBigDatasQuery(), CancellationToken.None);

        result.Should().HaveCount(2);
        result[0].PluginName.Should().Be("PluginA");
        result[0].UserCreater.Should().Be("user1");
        result[1].PluginName.Should().Be("PluginB");
        result[1].UserCreater.Should().Be("user2");
    }

    [Fact]
    public async Task Handle_WhenNoEntities_ShouldReturnEmptyList()
    {
        _repoMock
            .Setup(r => r.Entities)
            .Returns(Enumerable.Empty<PluginBigData>().AsAsyncQueryable());

        var result = await _sut.Handle(new GetPluginBigDatasQuery(), CancellationToken.None);

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_WhenDecompressThrows_ShouldSkipFailedEntity()
    {
        var goodBytes = Encoding.UTF8.GetBytes("{\"ok\":true}");
        var badBytes = new byte[] { 0x00, 0x01 };

        var entities = new List<PluginBigData>
        {
            new("user1", "GoodPlugin", goodBytes),
            new("user2", "BadPlugin", badBytes),
        };

        _repoMock
            .Setup(r => r.Entities)
            .Returns(entities.AsAsyncQueryable());

        _compressionMock.Setup(c => c.Decompress(goodBytes)).Returns(goodBytes);
        _compressionMock.Setup(c => c.Decompress(badBytes)).Throws<InvalidDataException>();

        var result = await _sut.Handle(new GetPluginBigDatasQuery(), CancellationToken.None);

        // Bad entity is swallowed by catch, only good one is in result
        result.Should().HaveCount(1);
        result[0].PluginName.Should().Be("GoodPlugin");
    }

    [Fact]
    public async Task Handle_ShouldFormatCreatedAtCorrectly()
    {
        var jsonBytes = Encoding.UTF8.GetBytes("{\"x\":1}");
        var entity = new PluginBigData("u", "Plugin", jsonBytes);

        _repoMock
            .Setup(r => r.Entities)
            .Returns(new List<PluginBigData> { entity }.AsAsyncQueryable());

        _compressionMock.Setup(c => c.Decompress(jsonBytes)).Returns(jsonBytes);

        var result = await _sut.Handle(new GetPluginBigDatasQuery(), CancellationToken.None);

        result[0].CreatedAt.Should().MatchRegex(@"^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}$");
    }

    [Fact]
    public async Task Handle_ShouldIncludeEntityId()
    {
        var jsonBytes = Encoding.UTF8.GetBytes("{\"x\":1}");
        var entity = new PluginBigData("u", "Plugin", jsonBytes);

        _repoMock
            .Setup(r => r.Entities)
            .Returns(new List<PluginBigData> { entity }.AsAsyncQueryable());

        _compressionMock.Setup(c => c.Decompress(jsonBytes)).Returns(jsonBytes);

        var result = await _sut.Handle(new GetPluginBigDatasQuery(), CancellationToken.None);

        Guid.TryParse(result[0].Id, out _).Should().BeTrue();
    }
}
