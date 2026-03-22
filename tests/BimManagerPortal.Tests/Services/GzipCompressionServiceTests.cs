using System.Text;
using BimManagerPortal.Persistance.ApiServices;

namespace BimManagerPortal.Tests.Services;

public class GzipCompressionServiceTests
{
    private readonly GzipCompressionService _sut = new();

    [Fact]
    public void Compress_ShouldReturnGzipMagicBytes()
    {
        var data = Encoding.UTF8.GetBytes("{\"key\":\"value\"}");

        var compressed = _sut.Compress(data);

        compressed[0].Should().Be(0x1F);
        compressed[1].Should().Be(0x8B);
    }

    [Fact]
    public void Compress_ShouldReduceSize_ForLargeInput()
    {
        var data = Encoding.UTF8.GetBytes(new string('a', 10_000));

        var compressed = _sut.Compress(data);

        compressed.Length.Should().BeLessThan(data.Length);
    }

    [Fact]
    public void Decompress_ShouldRestoreOriginalData()
    {
        var original = Encoding.UTF8.GetBytes("{\"hello\":\"world\",\"number\":42}");
        var compressed = _sut.Compress(original);

        var decompressed = _sut.Decompress(compressed);

        decompressed.Should().Equal(original);
    }

    [Fact]
    public void Decompress_WhenDataIsNotGzip_ShouldReturnAsIs()
    {
        var raw = Encoding.UTF8.GetBytes("plain text, not compressed");

        var result = _sut.Decompress(raw);

        result.Should().Equal(raw);
    }

    [Fact]
    public void Decompress_WhenNull_ShouldReturnNull()
    {
        var result = _sut.Decompress(null!);

        result.Should().BeNull();
    }

    [Fact]
    public void Decompress_WhenEmpty_ShouldReturnEmpty()
    {
        var result = _sut.Decompress([]);

        result.Should().BeEmpty();
    }

    [Fact]
    public void CompressDecompress_RoundTrip_ShouldBeIdempotent()
    {
        var json = Encoding.UTF8.GetBytes("{\"plugin\":\"TestPlugin\",\"version\":1,\"settings\":{\"enabled\":true}}");

        var roundTripped = _sut.Decompress(_sut.Compress(json));

        roundTripped.Should().Equal(json);
    }
}
