using System.Diagnostics;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Trace;

class Program
{
    static async Task Main(string[] args)
    {
        using var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .AddConsoleExporter() // This will export the traces to the console.
            .AddOtlpExporter(p =>
            {
               // p.Endpoint = new Uri("https://127.0.0.1:4317");
            })
            .AddSource("DemoSource")
            .Build();

        var activitySource = new ActivitySource("DemoSource");
        // for (int i = 0; i < 10; i++)
        {


            // Creating a span
            using (var activity = activitySource.StartActivity("DemoActivity"))
            {
                // Simulate some work
                Console.WriteLine("Hello, OpenTelemetry!");

                // Optionally, you can set attributes related to the work being done.
                activity?.SetTag("foo", "bar");
            }

            await Task.Delay(3_000);
        }
        // The using statement ensures the span is ended correctly and the tracer provider is disposed of,
        // which will flush any remaining data to the exporter.
    }
}