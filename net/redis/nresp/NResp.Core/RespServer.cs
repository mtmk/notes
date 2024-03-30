using System.Buffers;
using System.IO.Pipelines;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NResp.Core;

public class RespServer
{
    private TcpListener? _tcpListener;
    private Task? _acceptTask;
    private CancellationTokenSource? _cts;
    private Dictionary<uint, RespClient> _clients = new();

    public void Start(string host, int port)
    {
        _cts = new CancellationTokenSource();
        _tcpListener = new TcpListener(IPAddress.Parse(host), port);
        _tcpListener.Start();
        _acceptTask = Task.Run(AcceptClients);
    }

    private async Task AcceptClients()
    {
        if (_tcpListener is not { } tcpListener
            || _cts is not { } cts)
            throw new RespException("Listener is not initialized");
        
        uint clientId = 0;
        while (cts.IsCancellationRequested == false)
        {
            var client = await tcpListener.AcceptTcpClientAsync(cts.Token);
            
            clientId++;
            
            var clientTask = Task.Run(async () => await HandleClient(clientId, client, cts.Token));
            
            lock(_clients)
                _clients.Add(clientId, new RespClient
                {
                    Server = this,
                    ClientId = clientId,
                    Client = client,
                    Task = clientTask,
                });
        }
    }

    private async Task HandleClient(uint clientId, TcpClient client, CancellationToken cancellationToken)
    {
        var writer = PipeWriter.Create(client.GetStream());
        var reader = PipeReader.Create(client.GetStream());

        while (cancellationToken.IsCancellationRequested == false)
        {
            var result = await reader.ReadAsync(cancellationToken);
            var buffer = result.Buffer;
            var consumed = buffer.Start;
            var examined = buffer.Start;
            try
            {
                if (buffer.IsEmpty == false)
                {
                    var lf = buffer.PositionOf((byte)'\n');
                    if (lf != null)
                    {
                        var line = buffer.Slice(0, lf.Value);
                        Console.WriteLine($"{DateTime.Now:HH:mm:ss} [{clientId}] rcv: >>{Encoding.ASCII.GetString(line.ToArray())}<<");
                        examined = buffer.GetPosition(1, lf.Value);
                        consumed = buffer.GetPosition(1, lf.Value);
                    }
                }
            }
            finally
            {
                reader.AdvanceTo(consumed, examined);
            }
        }
        
    }
}

internal class RespClient
{
    public uint ClientId { get; set; }
    public TcpClient Client { get; set; }
    public Task Task { get; set; }
    public RespServer Server { get; set; }
}

public class RespException(string message) : Exception(message);
