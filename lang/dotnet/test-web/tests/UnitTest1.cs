using System;
using System.Threading.Tasks;
using Grpc.Net.Client;
using GrpcService1;
using GrpcService1.Services;
using NUnit.Framework;

namespace TestProject1;

public class Tests
{
    private GreeterServer _server = new();
    
    [SetUp]
    public async Task Setup() => await _server.Start();

    [TearDown]
    public async Task TearDown() => await _server.Stop();

    [Test]
    public void Test1()
    {
        var client = new Greeter.GreeterClient(GrpcChannel.ForAddress(_server.Url));
        var reply = client.SayHello(new HelloRequest {Name = "Bob"});
        Console.WriteLine(reply);
    }
}