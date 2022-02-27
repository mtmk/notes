using Grpc.Net.Client;
using GrpcService1;

var client = new Greeter.GreeterClient(GrpcChannel.ForAddress(args[0]));
var reply = client.SayHello(new HelloRequest
{
    Name = "Bob"
});

Console.WriteLine(reply);
