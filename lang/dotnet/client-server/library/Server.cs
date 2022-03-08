using System.Buffers;
using System.Net;
using System.Net.Sockets;

namespace library;

public class Server
{
    private TcpListener? _tcpListener;
    private Thread? _thread;
    private readonly ArrayPool<byte> _pool;
    private const int FrameSize = 8;
    
    public Server()
    {
        _pool = ArrayPool<byte>.Create(FrameSize, 128);
    }

    public Server Start(bool verbose = false)
    {
        _tcpListener = new TcpListener(IPAddress.Loopback, 1234);
        _tcpListener.Start();
        _thread = new Thread(_ =>
        {
            var tcpClient = _tcpListener.AcceptTcpClient();
            tcpClient.NoDelay = true;
            Task.Run(async () =>
            {
                var stream = tcpClient.GetStream();
                while (true)
                {
                    int data1;
                    int data2;
                    
                    var start = 0;
                    var len = FrameSize;
                    var buffer = _pool.Rent(FrameSize);
                    try
                    {
                        do
                        {
                            var read = await stream.ReadAsync(buffer.AsMemory(start, len)).ConfigureAwait(false);
                            start += read;
                            len -= read;
                        } while (len > 0);

                        data1 = BitConverter.ToInt32(buffer.AsSpan(0, 4));
                        data2 = BitConverter.ToInt32(buffer.AsSpan(4, 4));
                    }
                    finally
                    {
                        _pool.Return(buffer);
                    }
                    
                    stream.WriteByte(1);

                    if (verbose)
                    {
                        Console.Write(data1);
                        Console.Write(',');
                        Console.Write(data2);
                        Console.WriteLine();
                    }
                }
            });
        }) { IsBackground = true };
        
        _thread.Start();

        return this;
    }
}