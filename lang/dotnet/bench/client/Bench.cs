using System.Net;
using System.Net.Sockets;
using BenchmarkDotNet.Attributes;

namespace client;

[MemoryDiagnoser]
public class Bench
{
    private readonly NetworkStream _stream;

    public Bench()
    {
        var client = new TcpClient();
        client.Connect(IPAddress.Loopback, 1234);
        _stream = client.GetStream();
    }

    [Benchmark]
    public int ReadWrite()
    {
        _stream.WriteByte(1);
        var readByte = _stream.ReadByte();
        return readByte;
    }
}