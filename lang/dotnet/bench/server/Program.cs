using System.Net;
using System.Net.Sockets;

var listener = new TcpListener(IPAddress.Loopback, 1234);
listener.Start();

Console.WriteLine($"Listening..");

var stream = listener.AcceptTcpClient().GetStream();

Console.WriteLine($"Client connected..");

while (true)
{
    var readByte = stream.ReadByte();
    stream.WriteByte((byte)readByte);
}
