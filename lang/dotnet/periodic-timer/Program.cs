using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Channels;

var periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(2));

var cts = new CancellationTokenSource();

var channel = Channel.CreateBounded<int>(capacity: 2);

var inputLoop = Task.Run(action: delegate
{
    while (true)
    {
        var cmd = Console.ReadLine();
        if (cmd == null || Regex.IsMatch(cmd, @"^(?:quit|exit|.q)$"))
        {
            break;
        }
        else if (Regex.IsMatch(cmd, @"^r\s*\d+"))
        {
            var count = int.Parse(Regex.Match(cmd, @"^r\s*(\d+)").Groups[1].Value);
            for (int j = 0; j < count; j++)
            {
                if (channel.Reader.TryRead(out var read))
                {
                    Log($"READ:{read}");
                }
                else
                {
                    Log("Nothing to read");
                }
            }
        }
    }

    cts.Cancel();
});

int i = 0;
var sw = new Stopwatch();
sw.Restart();
while (!cts.IsCancellationRequested)
{
    try
    {
        // Log($"Write:{i}");
        await channel.Writer.WriteAsync(i++, cts.Token);

        // Log($"Timer");

        await periodicTimer.WaitForNextTickAsync(cts.Token);
        Log($"tick {sw.Elapsed.TotalSeconds:f}s");
        sw.Restart();
    }
    catch (OperationCanceledException)
    {
    }
}

await inputLoop;

Log("Bye");

void Log(string message)
{
    Console.WriteLine($"{DateTime.Now:mm:ss.fff} [{Environment.CurrentManagedThreadId:d3}] {message}");
}