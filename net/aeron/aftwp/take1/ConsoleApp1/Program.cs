// See https://aka.ms/new-console-template for more information

using System.Text;
using Adaptive.Aeron;
using Adaptive.Aeron.LogBuffer;
using Adaptive.Agrona;
using Adaptive.Agrona.Concurrent;

Console.WriteLine("Hello, World!");

var dir = "/Users/ziya/tmp/take1";


var context = new Aeron.Context().AeronDirectoryName(dir);
var aeron = Aeron.Connect(context);
var subUri = "aeron:udp?endpoint=127.0.0.1:4322|reliable=true";
var pubUri = "aeron:udp?endpoint=127.0.0.1:4321|reliable=true";
int ECHO_STREAM_ID = 0x2044f002;
var sub = aeron.AddSubscription(subUri, ECHO_STREAM_ID);
var pub = aeron.AddPublication(pubUri, ECHO_STREAM_ID);

var buffer = new UnsafeBuffer(BufferUtil.AllocateDirectAligned(2048, 16));

var random = new Random();

while (true)
{
    if (pub.IsConnected)
    {
        if (SendMessage(pub, buffer, "HELLO 4322"))
        {
            break;
        }
    }
    Thread.Sleep(1000);
}

var assembler = new FragmentAssembler(OnParseMessage);

while (true)
{
    if (pub.IsConnected)
    {
        SendMessage(pub, buffer, random.Next().ToString());
    }

    if (sub.IsConnected)
    {
        sub.Poll(assembler, 10);
    }
    Thread.Sleep(1000);
}

static void OnParseMessage(IDirectBuffer buffer, int offset, int length, Header header)
{
    var bytes = new byte[length];
    buffer.GetBytes(offset, bytes);
    var response = Encoding.UTF8.GetString(bytes);
    Console.WriteLine($"response: {response}");
}

static bool SendMessage(Publication? pub, UnsafeBuffer buffer, string text)
{
    Console.WriteLine($"Send {text}");
    var bytes = Encoding.UTF8.GetBytes(text);
    buffer.PutBytes(0, bytes);
    long result = 0;
    for (int i = 0; i < 5; i++)
    {
        result = pub.Offer(buffer, 0, bytes.Length);
        if (result >= 0) return true;
        Thread.Sleep(1000);
    }

    Console.WriteLine($"Error: Can't send: {result}");
    return false;
}