using System.Text;
using System.Text.Json;
using BimManagerPortal.Application.Features.PluginBigDatas.Queries.GetPluginBigData;
using BimManagerPortal.Application.Interfaces.Compress;
using BimManagerPortal.Application.Interfaces.Repositories;
using BimManagerPortal.Domain.Entities.BigDataPlugins;
using BimManagerPortal.Shared.Dtos.PluginBigDatas;
using Microsoft.Extensions.Logging;

namespace BimManagerPortal.Tests.Handlers;

public class GetPluginBigDataQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ICompressionService> _compressionMock = new();
    private readonly Mock<IGenericRepository<PluginBigData>> _repoMock = new();
    private readonly Mock<ILogger<GetPluginBigDataQueryHandler>> _loggerMock = new();

    private readonly GetPluginBigDataQueryHandler _sut;

    public GetPluginBigDataQueryHandlerTests()
    {
        _unitOfWorkMock
            .Setup(u => u.Repository<PluginBigData>())
            .Returns(_repoMock.Object);

        _sut = new GetPluginBigDataQueryHandler(
            _unitOfWorkMock.Object,
            _compressionMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnDecompressedJson()
    {
        var entityId = Guid.NewGuid();
        var originalJson = "{\"setting\":\"value\"}";
        var jsonBytes = Encoding.UTF8.GetBytes(originalJson);

        var entity = new PluginBigData("creator", "MyPlugin", jsonBytes);

        _repoMock
            .Setup(r => r.GetByIdAsync(entityId.ToString()))
            .ReturnsAsync(entity);

        _compressionMock
            .Setup(c => c.Decompress(jsonBytes))
            .Returns(jsonBytes);

        var query = new GetPluginBigDataQuery(new GetPluginBigDataRequestDto(entityId.ToString()));

        var result = await _sut.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Json.GetProperty("setting").GetString().Should().Be("value");
    }

    [Fact]
    public async Task Handle_ShouldCallDecompressOnEntityData()
    {
        var entityId = Guid.NewGuid();
        var compressedData = new byte[] { 0x1F, 0x8B, 0x08 };
        var jsonBytes = Encoding.UTF8.GetBytes("{\"a\":1}");

        var entity = new PluginBigData("user", "Plugin", compressedData);

        _repoMock
            .Setup(r => r.GetByIdAsync(entityId.ToString()))
            .ReturnsAsync(entity);

        _compressionMock
            .Setup(c => c.Decompress(compressedData))
            .Returns(jsonBytes);

        var query = new GetPluginBigDataQuery(new GetPluginBigDataRequestDto(entityId.ToString()));

        await _sut.Handle(query, CancellationToken.None);

        _compressionMock.Verify(c => c.Decompress(compressedData), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenDecompressThrows_ShouldRethrow()
    {
        var entityId = Guid.NewGuid();
        var entity = new PluginBigData("user", "Plugin", [0x00]);

        _repoMock
            .Setup(r => r.GetByIdAsync(entityId.ToString()))
            .ReturnsAsync(entity);

        _compressionMock
            .Setup(c => c.Decompress(It.IsAny<byte[]>()))
            .Throws<InvalidDataException>();

        var query = new GetPluginBigDataQuery(new GetPluginBigDataRequestDto(entityId.ToString()));

        await _sut.Invoking(h => h.Handle(query, CancellationToken.None))
            .Should().ThrowAsync<InvalidDataException>();
    }
}
