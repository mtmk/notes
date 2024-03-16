// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OtelDemoCommon;

var serviceName = "DemoClient";
var serviceVersion = "1.0.0"; 

using var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .AddConsoleExporter() // This will export the traces to the console.
    .AddOtlpExporter()
    .SetResourceBuilder(ResourceBuilder.CreateDefault()
        .AddService(serviceName: serviceName, serviceVersion: serviceVersion))
    .AddSource("DemoClientSource")
    .AddSource("MyClientSource")
    .Build();

ActivitySource activitySource = new("MyClientSource");

Console.WriteLine("OTEL Demo Client");

var client = new DemoClient();

using (var activity = activitySource.StartActivity("SayHello"))
{
    var response = await client.SendMessageAsync(new MessageData
    {
        Text = "Hello, OTEL!",
        Headers =
        [
            new("X-OTEL-DEMO", "client-message"),
        ]
    });

    Console.WriteLine($"[RCV] {response}");
}

