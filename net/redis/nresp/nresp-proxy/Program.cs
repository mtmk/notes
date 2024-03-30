
using NResp.Proxy;

var server = new RespProxyServer("192.168.0.21", 6379);

server.Start("127.0.0.1", 6379);

while (true)
    Console.ReadLine();
