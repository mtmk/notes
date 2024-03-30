using System.Buffers;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NResp.Proxy;

public class RespProxyServer(string targetHost, int targetPort)
{
    private readonly object _logGate = new();
    private TcpListener? _tcpListener;
    private Task? _acceptTask;
    private CancellationTokenSource? _cts;
    private readonly Dictionary<int, Task> _clientTasks = new();

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
            throw new RespProxyException("Listener is not initialized");

        int clientId = 0;
        while (cts.IsCancellationRequested == false)
        {
            clientId++;
            var client = await tcpListener.AcceptTcpClientAsync(cts.Token);
            var clientTask = Task.Run(async () => await HandleClient(clientId, client, cts.Token));

            lock (_clientTasks)
                _clientTasks.Add(clientId, clientTask);
        }
    }

    private async Task HandleClient(int clientId, TcpClient client, CancellationToken cancellationToken)
    {
        try
        {
            var target = new TcpClient();
            try
            {
                await target.ConnectAsync(targetHost, targetPort, cancellationToken);
        
                await Task.WhenAll(
                    client.GetStream().CopyRespToAsync($"[{clientId}]->>", target.GetStream(), cancellationToken),
                    target.GetStream().CopyRespToAsync($"[{clientId}]<<-", client.GetStream(), cancellationToken)
                );
            }
            catch (OperationCanceledException)
            {
            }

            client.Close();
        
            lock (_clientTasks)
                _clientTasks.Remove(clientId);
        }
        catch (Exception e)
        {
            Log($"Client loop {clientId} error: {e}");
        }
    }

    private void Log(string message)
    {
        lock (_logGate)
        {
            Console.WriteLine($"{DateTime.Now:HH:mm:ss} {message}");
        }
    }
}

public static class RespStreamExtensions
{
    public static Task CopyRespToAsync(this NetworkStream source, string dir, NetworkStream destination, CancellationToken cancellationToken)
    {
        int bufferSize = 4096;
        return Core(dir, source, destination, bufferSize, cancellationToken);

        static async Task Core(string dir, Stream source, Stream destination, int bufferSize, CancellationToken cancellationToken)
        {
            byte[] buffer = ArrayPool<byte>.Shared.Rent(bufferSize);
            try
            {
                int bytesRead;
                while ((bytesRead = await source.ReadAsync(new Memory<byte>(buffer), cancellationToken).ConfigureAwait(false)) != 0)
                {
                    var readOnlyMemory = new ReadOnlyMemory<byte>(buffer, 0, bytesRead);
                    Console.WriteLine($"{dir} {Encoding.ASCII.GetString(readOnlyMemory.Span)}");
                    await destination.WriteAsync(readOnlyMemory, cancellationToken).ConfigureAwait(false);
                }
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }
    }
}
public class RespProxyException(string message) : Exception(message);
