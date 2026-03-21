using System.Text.Json;
using AutoMapper;
using BimManagerPortal.Application.Features.PluginBigDatas.Commands.PostPluginBigData;
using BimManagerPortal.Application.Interfaces.Compress;
using BimManagerPortal.Application.Interfaces.Repositories;
using BimManagerPortal.Domain.Entities.BigDataPlugins;

namespace BimManagerPortal.Tests.Handlers;

public class PostPluginBigDataCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<ICompressionService> _compressionMock = new();
    private readonly Mock<IGenericRepository<PluginBigData>> _repoMock = new();

    private readonly GetAllPlayersQueryHandler _sut;

    public PostPluginBigDataCommandHandlerTests()
    {
        _unitOfWorkMock
            .Setup(u => u.Repository<PluginBigData>())
            .Returns(_repoMock.Object);

        _sut = new GetAllPlayersQueryHandler(
            _unitOfWorkMock.Object,
            _mapperMock.Object,
            _compressionMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCompressJsonAndSaveEntity()
    {
        var jsonData = JsonDocument.Parse("{\"key\":\"value\"}").RootElement;
        var dto = new PostPluginBigDataRequestDto("TestPlugin", jsonData, "user@test.com");
        var command = new PostPluginBigDataCommand(dto);

        var compressedBytes = new byte[] { 0x1F, 0x8B, 0x01 };
        _compressionMock
            .Setup(c => c.Compress(It.IsAny<byte[]>()))
            .Returns(compressedBytes);

        PluginBigData? capturedEntity = null;
        _repoMock
            .Setup(r => r.AddAsync(It.IsAny<PluginBigData>()))
            .Callback<PluginBigData>(e => capturedEntity = e)
            .ReturnsAsync((PluginBigData e) => e);

        _unitOfWorkMock
            .Setup(u => u.SaveAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        await _sut.Handle(command, CancellationToken.None);

        _compressionMock.Verify(c => c.Compress(It.IsAny<byte[]>()), Times.Once);
        _repoMock.Verify(r => r.AddAsync(It.IsAny<PluginBigData>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);

        capturedEntity.Should().NotBeNull();
        capturedEntity!.PluginName.Should().Be("TestPlugin");
        capturedEntity.UserCreater.Should().Be("user@test.com");
        capturedEntity.JsonData.Should().Equal(compressedBytes);
    }

    [Fact]
    public async Task Handle_ShouldSerializeJsonBeforeCompressing()
    {
        var jsonData = JsonDocument.Parse("{\"x\":1}").RootElement;
        var dto = new PostPluginBigDataRequestDto("Plugin", jsonData, "admin");
        var command = new PostPluginBigDataCommand(dto);

        byte[]? capturedBytes = null;
        _compressionMock
            .Setup(c => c.Compress(It.IsAny<byte[]>()))
            .Callback<byte[]>(b => capturedBytes = b)
            .Returns([]);

        _repoMock.Setup(r => r.AddAsync(It.IsAny<PluginBigData>())).ReturnsAsync((PluginBigData e) => e);
        _unitOfWorkMock.Setup(u => u.SaveAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        await _sut.Handle(command, CancellationToken.None);

        capturedBytes.Should().NotBeNull();
        capturedBytes.Should().NotBeEmpty();
        // Bytes should be valid JSON
        var json = JsonSerializer.Deserialize<JsonElement>(capturedBytes!);
        json.GetProperty("x").GetInt32().Should().Be(1);
    }
}
