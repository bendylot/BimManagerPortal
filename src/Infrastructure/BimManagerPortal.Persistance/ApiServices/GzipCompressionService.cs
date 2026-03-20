using System.IO.Compression;
using BimManagerPortal.Application.Interfaces.Compress;

namespace BimManagerPortal.Persistance.ApiServices;

public class GzipCompressionService : ICompressionService
{
    public byte[] Compress(byte[] data)
    {
        using var output = new MemoryStream();
        using (var gzip = new GZipStream(output, CompressionLevel.Optimal))
        {
            gzip.Write(data, 0, data.Length);
        }

        return output.ToArray();
    }

    public byte[] Decompress(byte[] data)
    {
        if (data == null || data.Length == 0)
            return data;

        // GZip magic bytes: 0x1F 0x8B
        bool isGzip = data.Length >= 2 
                      && data[0] == 0x1F 
                      && data[1] == 0x8B;

        if (!isGzip)
        {
            // Данные не сжаты — вернуть как есть
            return data;
        }

        using var input = new MemoryStream(data);
        using var gzip = new GZipStream(input, CompressionMode.Decompress);
        using var output = new MemoryStream();
        gzip.CopyTo(output);
        return output.ToArray();
    }
}