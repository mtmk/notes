using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace OtelDemoCommon;

public class DemoClient
{
    ActivitySource _activitySource = new("DemoClientSource");
    private readonly TcpClient _tcpClient;
    private readonly NetworkStream _networkStream;
    private readonly StreamReader _reader;
    private readonly StreamWriter _writer;

    public DemoClient()
    {
        _tcpClient = new TcpClient();
        _tcpClient.Connect("127.0.0.1", 4444);
        _networkStream = _tcpClient.GetStream();
        _reader = new StreamReader(_networkStream, Encoding.ASCII);
        _writer = new StreamWriter(_networkStream, Encoding.ASCII);
    }

    public async Task<MessageData?> SendMessageAsync(MessageData message)
    {
        var tags = new List<KeyValuePair<string, object?>>
        {
            new("x", 1),
        };
        using var activity = _activitySource.StartActivity(
            "SendMessage",
            kind: ActivityKind.Client,
            parentContext: default,
            tags: tags);
        
        DistributedContextPropagator.Current.Inject(
            activity: activity,
            carrier: message,
            setter: static (carrier, fieldName, fieldValue) =>
            {
                if (carrier is not MessageData messageData)
                {
                    Debug.Assert(false, "This should never be hit.");
                    return;
                }

                messageData.Headers.Add(new KeyValuePair<string, string>(fieldName, fieldValue));
            });
        
        await _writer.WriteLineAsync(JsonSerializer.Serialize(message));
        await _writer.FlushAsync();
        var lineAsync = await _reader.ReadLineAsync();
        if (lineAsync == null)
            return null;
        var data = JsonSerializer.Deserialize<MessageData>(lineAsync);
        
        data.TryParseTraceContext(out var context);
        var tags1 = new List<KeyValuePair<string, object?>>
        {
            new("x", 1),
        };

        using var activity1 = _activitySource.StartActivity(
            "ReceivedMessage",
            kind: ActivityKind.Client,
            parentContext: context, // propagate from current activity
            tags: tags1);
        
        return data;
    }
}