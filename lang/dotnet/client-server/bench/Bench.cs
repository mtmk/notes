using BenchmarkDotNet.Attributes;
using library;

namespace bench;

[MemoryDiagnoser]
public class Bench
{
    private readonly Client _client;
    private int _data1;
    private int _data2;
    
    public Bench()
    {
        var server = new Server().Start();
        _client = new Client().Start(server.Port);
    }

    [Benchmark]
    public int Send()
    {
        _data1 += 1;
        _data2 += 2;
        return _client.Send(_data1, _data2).Result;
    }
}
