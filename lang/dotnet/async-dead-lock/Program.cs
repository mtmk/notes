using System.Runtime.InteropServices;

class P
{
    static void Main()
    {
        Print("start");
        
        DeadLock().Wait();
        
        Print("bye");
    }

    static void Print(string message)
    {
        Console.WriteLine($"{DateTime.Now:mm:ss.fff} [{Thread.CurrentThread.ManagedThreadId}] {message}");
    }

    private static ManualResetEventSlim _mutex = new();

    public static async Task DeadLock()
    {
        Print("dead lock");
        await ProcessAsync();

        Print("dead lock mutex wait");
        _mutex.Wait();
    }

    private static Task ProcessAsync()
    {
        Print("process");
        
        // !!! DEAD LOCK !!!
        // Runs continuation on the same thread
        // var tcs = new TaskCompletionSource();
        
        // It works!
        // Runs continuation on the thread pool
        var tcs = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        
        Task.Run(() =>
        {
            Print("process task start");
            Thread.Sleep(1000);

            Print("process set result");
            tcs.SetResult();

            Print("process mutex set");
            _mutex.Set();

            Print("process task end");
        });
        
        Print("process return task");
        return tcs.Task;
    }
}