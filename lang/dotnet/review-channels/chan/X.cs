namespace review.channels.chan;

public class X
{
    public static void Log(object? logMessage = default,
        [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
    {
        Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff} [{memberName}:{sourceLineNumber}] {logMessage}");
    }
}