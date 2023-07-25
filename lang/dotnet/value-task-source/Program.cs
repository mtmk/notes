using System.Threading.Tasks.Sources;

class X
{
    static async Task Main()
    {
        Log("Starting...");
        Log("(See thread IDs)");

        var a = new A(false);
        var cts = new CancellationTokenSource();
        var mres = new ManualResetEventSlim();
        ThreadPool.UnsafeQueueUserWorkItem(_ =>
        {
            while (!cts.IsCancellationRequested)
            {
                Log("T:Tick");
                Thread.Sleep(1000);
                a.Signal();
            }

            Log("T:done");
            mres.Set();
        }, null);

        Log("X:await a.CallAsync() - 1");
        await a.CallAsync();
        Log("X:done - 1");

        Log("X:await a.CallAsync() - 2");
        await a.CallAsync();
        Log("X:done - 2");

        cts.Cancel();
        mres.Wait();

        Log("Bye");
    }
    
    public static void Log(string message)
    {
        Console.WriteLine($"{DateTime.Now:mm:ss.fff} [{Thread.CurrentThread.ManagedThreadId}] {message}");
    }
}

public class A : IValueTaskSource
{
    private ManualResetValueTaskSourceCore<bool> _mrvtsc;
    private bool _signaled;

    public A(bool runContinuationsAsynchronously)
    {
        _mrvtsc = new ManualResetValueTaskSourceCore<bool>
        {
            RunContinuationsAsynchronously = runContinuationsAsynchronously
        };
    }

    public ValueTask CallAsync()
    {
        X.Log("A:CallAsync");
        return new ValueTask(this, _mrvtsc.Version);
    }

    public void Signal()
    {
        lock (this)
        {
            if (_signaled) return;
            _signaled = true;
        }
        X.Log("A:Signal");
        X.Log("A:_mrvtsc.SetResult");
        _mrvtsc.SetResult(true);
    }
    
    public void GetResult(short token)
    {
        X.Log($"A:GetResult token={token}");

        lock (this)
        {
            try
            {
                X.Log($"A:_mrvtsc.GetResult({token})");
                _mrvtsc.GetResult(token);
            }
            finally
            {
                X.Log($"A:_mrvtsc.Reset()");
                _mrvtsc.Reset();
                _signaled = false;
            }
        }
    }

    public ValueTaskSourceStatus GetStatus(short token)
    {
        X.Log($"A:GetStatus token={token}");
        var status = _mrvtsc.GetStatus(token);
        X.Log($"A:_mrvtsc.GetStatus({token}) => {status}");
        return status;
    }

    public void OnCompleted(Action<object?> continuation, object? state, short token, ValueTaskSourceOnCompletedFlags flags)
    {
        X.Log($"A:OnCompleted token={token} flags={flags}");
        X.Log($"A:_mrvtsc.OnCompleted(continuation, state, token:{token} flags:{flags}");
        _mrvtsc.OnCompleted(continuation, state, token, flags);
    }
}
