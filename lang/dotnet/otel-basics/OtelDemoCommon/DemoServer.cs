using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace OtelDemoCommon;

public class DemoServer
{
    ActivitySource _activitySource = new("DemoServerSource");

    private readonly Func<Msg, CancellationToken, Task> _handler;
    Task? _acceptTask;
    Dictionary<int, Task> _clientTasks = new();
    private TcpListener? _tcpListener;
    private CancellationTokenSource? _cts;

    public DemoServer(Func<Msg, CancellationToken, Task> handler)
    {
        _handler = handler;
    }

    public Task Start()
    {
        _cts = new CancellationTokenSource();
        _tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 4444);
        _tcpListener.Start();
        _acceptTask = Task.Run(AcceptTask);
        return Task.CompletedTask;
    }

    public async Task Stop()
    {
        _tcpListener?.Stop();
        if (_cts != null)
            await _cts.CancelAsync();

        Dictionary<int, Task>.ValueCollection tasks;
        lock (_clientTasks)
        {
            tasks = _clientTasks.Values;
        }

        foreach (var clientTask in tasks)
            try
            {
                await clientTask.WaitAsync(TimeSpan.FromSeconds(3));
            }
            catch
            {
                // ignored
            }
        
        if (_acceptTask != null)
            try
            {
                await _acceptTask.WaitAsync(TimeSpan.FromSeconds(3));
            }
            catch
            {
                // ignored
            }
    }

    private async Task AcceptTask()
    {
        try
        {
            if (_cts is not { } cts)
                throw new Exception("Cancellation token source is null.");
            if (_tcpListener is not {} tcpListener)
                throw new Exception("TCP listener is null.");

            var clientId = 0;
            while (cts.Token.IsCancellationRequested == false)
            {
                var tcpClient = await tcpListener.AcceptTcpClientAsync();
                clientId++;
                lock(_clientTasks)
                    _clientTasks.Add(clientId, Task.Run(async () => await HandleClient(clientId, tcpClient, cts.Token)));
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Accept loop error: {e}");
        }
    }

    private async Task HandleClient(int id, TcpClient tcpClient, CancellationToken ctsToken)
    {
        Console.WriteLine($"[CLS] Client connected.");
        var stream = tcpClient.GetStream();
        var reader = new StreamReader(stream, Encoding.ASCII);
        var writer = new StreamWriter(stream, Encoding.ASCII);
        while (ctsToken.IsCancellationRequested == false)
        {
            var line = await reader.ReadLineAsync(ctsToken);
            if (line == null)
                break;
            try
            {
                MessageData data;
                Exception exception;
                try
                {
                    data = JsonSerializer.Deserialize<MessageData>(line) ?? throw new Exception("null message");
                    exception = null;
                }
                catch(Exception e)
                {
                    exception = e;
                    data = new MessageData();
                }

                data.TryParseTraceContext(out var context);
                
                var tags = new List<KeyValuePair<string, object?>>
                {
                    new("x", 1),
                };

                using var activity = _activitySource.StartActivity(
                    "ReceivedMessage",
                    kind: ActivityKind.Server,
                    parentContext: context, // propagate from current activity
                    tags: tags);
                
                await _handler(new Msg
                {
                    Message = data,
                    Exception = exception,
                    Writer = writer,
                    CancellationToken = ctsToken,
                }, ctsToken);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Handler error: {e}");
            }
        }
        Console.WriteLine($"[CLS] Client disconnected.");
        
        lock(_clientTasks)
            _clientTasks.Remove(id);
    }
    
    
}

public class Msg
{
    public Exception? Exception { get; set; }
    public MessageData Message { get; set; }
    public StreamWriter Writer { get; set; }
    public CancellationToken CancellationToken { get; set; }

    public async Task ReplyAsync(MessageData messageData)
    {
        if (Exception != null)
            throw Exception;
        await Writer.WriteLineAsync(JsonSerializer.Serialize(messageData));
        await Writer.FlushAsync(CancellationToken);
    }
}