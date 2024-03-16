// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OtelDemoCommon;

var serviceName = "DemoServer";
var serviceVersion = "1.0.0";

using var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .AddConsoleExporter() // This will export the traces to the console.
    .AddOtlpExporter()
    .SetResourceBuilder(ResourceBuilder.CreateDefault()
        .AddService(serviceName: serviceName, serviceVersion: serviceVersion))
    .AddSource("DemoServerSource")
    .Build();

Console.WriteLine("OTEL Demo Server");

var server = new DemoServer(async (msg, ct) =>
{
    Activity.Current?.SetTag("my-server-tag", "my-server-tag-value");
    Console.WriteLine($"[RCV] {msg.Message}");
    await msg.ReplyAsync(new MessageData
    {
        Text = "ACK"
    });
});

await server.Start();

Console.WriteLine("Press enter to stop the server.");

Console.ReadLine();

await server.Stop();
