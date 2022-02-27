using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GrpcService1.Services;

public class GreeterServer
{
    private readonly WebApplication _app;

    public GreeterServer()
    {
        var builder = WebApplication.CreateBuilder();
        builder.Services.AddGrpc();
        builder.WebHost.ConfigureKestrel(k =>
        {
            k.Listen(IPAddress.Parse("127.0.0.1"), 0, op =>
            {
                op.Protocols = HttpProtocols.Http2;
            });
        });
        _app = builder.Build();
        _app.MapGrpcService<GreeterService>();
        //app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
        //app.Run();
    }
    
    public async Task Start() => await _app.StartAsync();
    public string Url => _app.Urls.First();
    public async Task Wait() => await _app.WaitForShutdownAsync();
    public async Task Stop() => await _app.StopAsync();
}