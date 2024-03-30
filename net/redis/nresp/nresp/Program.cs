using NResp.Core;

var server = new RespServer();

server.Start("127.0.0.1", 6379);

Console.WriteLine("Server started.");

while (true)
{
    Console.ReadLine();
}
