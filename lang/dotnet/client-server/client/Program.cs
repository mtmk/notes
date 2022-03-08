// See https://aka.ms/new-console-template for more information

using library;

Console.WriteLine("Starting client...");

var client = new Client().Start();

int data1 = 0;
int data2 = 0;

while (true)
{
    await Task.Delay(1_000);
    data1 += 1;
    data2 += 2;
    var ack = await client.Send(data1, data2);
    Console.WriteLine(ack);
}
