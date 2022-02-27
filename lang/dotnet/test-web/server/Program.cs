using GrpcService1.Services;

var server = new GreeterServer();

await server.Start();

Console.WriteLine(server.Url);

await server.Wait();

await server.Stop();
