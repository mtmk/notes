using System;
using System.Threading;
using JetBrains.dotMemoryUnit;
using library;
using NUnit.Framework;

namespace tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        var checkPoint = dotMemory.Check();
        
        var server = new Server().Start();
        
        Thread.Sleep(1_000);
        
        var client = new Client().Start(server.Port);
        
        Thread.Sleep(1_000);
        
        var ack = client.Send(0, 0).Result;
        
        Assert.True(ack == 1);
        
        dotMemory.Check(memory =>
        {
            foreach (var memoryInfo in memory.GetDifference(checkPoint).GetNewObjects().GroupByType())
            {
                Console.WriteLine($"{memoryInfo.Type.Name} {memoryInfo.ObjectsCount}");
            }
        });
    }
}