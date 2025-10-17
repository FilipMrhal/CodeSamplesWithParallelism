using System.Diagnostics;
using ParallelProgrammingSamples;

var sw = new Stopwatch();
for (var i = 0; i < 10; i++)
{
    sw.Start();
    _ = LockingAndThreadSafety.TryDeadlock();
    sw.Stop();
    Console.WriteLine(sw.Elapsed >= TimeSpan.FromSeconds(4) ? "Deadlock" : "Did not deadlock");
    Thread.Sleep(2000);
}