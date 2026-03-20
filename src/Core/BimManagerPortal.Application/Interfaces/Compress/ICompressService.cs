namespace BimManagerPortal.Application.Interfaces.Compress;

public interface ICompressionService
{
    byte[] Compress(byte[] data);
    byte[] Decompress(byte[] data);
}