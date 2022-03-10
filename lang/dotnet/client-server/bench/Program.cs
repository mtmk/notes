using bench;
using BenchmarkDotNet.Running;

var summary = BenchmarkRunner.Run<Bench>();

foreach (var report in summary.Reports)
{
    Console.WriteLine($"GcStats.TotalOperations: {report.GcStats.TotalOperations}");
    Console.WriteLine($"GcStats.GetTotalAllocatedBytes: {report.GcStats.GetTotalAllocatedBytes(true)}");
}