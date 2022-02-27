using System;
using System.Threading.Tasks;
using Grpc.Net.Client;
using GrpcService1;
using GrpcService1.Services;
using NUnit.Framework;

namespace TestProject1;

public class Tests
{
    private GreeterServer? _server;
    
    [SetUp]
    public async Task Setup() => await (_server = new GreeterServer()).Start();

    [TearDown]
    public async Task TearDown() => await _server!.Stop();

    [Test]
    public void Test1()
    {
        var client = new Greeter.GreeterClient(GrpcChannel.ForAddress(_server!.Url));
        var reply = client.SayHello(new HelloRequest {Name = "Bob1"});
        Console.WriteLine(reply);
        Assert.AreEqual("Hello Bob1", reply.Message);
    }
    
    [Test]
    public void Test2()
    {
        var client = new Greeter.GreeterClient(GrpcChannel.ForAddress(_server!.Url));
        var reply = client.SayHello(new HelloRequest {Name = "Bob2"});
        Console.WriteLine(reply);
        Assert.AreEqual("Hello Bob2", reply.Message);
    }
}