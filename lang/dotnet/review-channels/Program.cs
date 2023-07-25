// See https://aka.ms/new-console-template for more information

using review.channels.chan;

Console.WriteLine("Hello, World!");

var channel = new UnboundedChannel<Bar>(false);



var loop = Task.Run(async () =>
{
    while (await channel.Reader.WaitToReadAsync(CancellationToken.None).ConfigureAwait(false))
    {
        while (channel.Reader.TryRead(out var bar))
        {
            X.Log($"rcv:{bar}");
        }
    }
    X.Log($"end");
    // await foreach (var bar in channel.Reader.ReadAllAsync())
    // {
    //     Console.WriteLine($"rcv:{bar}");
    // }
});

// for (int i = 0; i < 10; i++)
var i = 1;
{
    await Task.Delay(1000);
    X.Log($"snd:{i}");
    await channel.Writer.WriteAsync(new Bar(i, $"name{i}"), CancellationToken.None);
}

channel.Writer.Complete();

await loop;
record Bar(int Id, string Name);
