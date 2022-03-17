using System.Buffers;
using BenchmarkDotNet.Attributes;

namespace simple;

[MemoryDiagnoser]
public class Bench
{
    [Benchmark]
    public byte[] NewBytes()
    {
        return new byte[10];
    }
    
    [Benchmark]
    public int ArrayPoolBytes()
    {
        var bytes = ArrayPool<byte>.Shared.Rent(10);
        try
        {
            return bytes.Length;
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(bytes);
        }
    }
}