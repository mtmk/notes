using System.Buffers;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace library;

public class Client
{
    private NetworkStream? _stream;
    private readonly ArrayPool<byte> _pool;
    private const int FrameSize = 8;
    
    public Client()
    {
        _pool = ArrayPool<byte>.Create(FrameSize, 128);
    }

    public Client Start(int port)
    {
        var tcpClient = new TcpClient();
        tcpClient.NoDelay = true;
        tcpClient.Connect(IPAddress.Loopback, port);
        _stream = tcpClient.GetStream();
        return this;
    }

    public async ValueTask<int> Send(int data1, int data2)
    {
        var buffer = _pool.Rent(FrameSize);
        try
        {
            Unsafe.As<byte, int>(ref buffer[0]) = data1;
            Unsafe.As<byte, int>(ref buffer[4]) = data2;
            await _stream!.WriteAsync(buffer.AsMemory(0, FrameSize)).ConfigureAwait(false);
            var ack = _stream.ReadByte();
            return ack;
        }
        finally
        {
            _pool.Return(buffer);
        }
    }
}