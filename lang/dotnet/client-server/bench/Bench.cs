using BenchmarkDotNet.Attributes;
using library;

namespace bench;

[MemoryDiagnoser]
public class Bench
{
    private readonly Client _client;
    private readonly Server _server;
    private int _data1;
    private int _data2;
    
    public Bench()
    {
        _server = new Server().Start();
        Thread.Sleep(3_000);
        _client = new Client().Start();
        Thread.Sleep(1_000);
        _ = _client.Send(0, 0).Result;
        Thread.Sleep(1_000);
    }

    [Benchmark]
    public int Send()
    {
        _data1 += 1;
        _data2 += 2;
        return _client.Send(_data1, _data2).Result;
    }
}
