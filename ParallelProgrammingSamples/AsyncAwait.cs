using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelProgrammingSamples;

public class AsyncAwait
{
    public async Task TestAsync()
    {
        var a = "sdgsdg";
        await TestWithReturnValueAsync();
        Console.WriteLine(a);
    }

    // public async void TestAsyncVoid()
    // {
    //     var currentcontext = TaskScheduler.Current;
    // }

    public async Task<int> TestWithReturnValueAsync()
    {
        return 0;
    }
}

public class MySynchroContext : SynchronizationContext
{
    public override void Post(SendOrPostCallback d, object state)
    {
        //Here we can for example limit the amount of concurrent executions. 
        base.Post(d, state);
    }
}

public class ConfigureAwaitFalse
{
    public async Task ConfigureAwaitFalseTest()
    {
        var aa = new AsyncAwait();
        await aa.TestAsync().ConfigureAwait(false);
        aa.TestAsync().GetAwaiter().GetResult();
        aa.TestAsync().Wait();
        Task.Run(() => { return;}).RunSynchronously();
    }
}

// public struct MyAsyncStateMachineMethodBuilder : AsyncTaskMethodBuilder
// {
//     public MyAsyncStateMachineMethodBuilder()
//     {
//         
//     }
// }