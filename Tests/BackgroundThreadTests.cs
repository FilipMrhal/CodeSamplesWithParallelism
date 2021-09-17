using System.Diagnostics;
using ParallelProgrammingSamples;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    public class BackgroundThreadTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public BackgroundThreadTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void GetOddNumbersWithoutThreadingTest()
        {
            var sw = new Stopwatch();
            sw.Start();
            var result = BackgroundThread.GetOddNumbersWithoutThreading();
            sw.Stop();
            _testOutputHelper.WriteLine("Elapsed " + sw.ElapsedMilliseconds);
            Assert.All(result, i =>
            {
                Assert.Equal(1, i % 2);
            });
            Assert.Equal(1000000,result.Count);
        }
        
        [Fact]
        public void GetOddNumbersMainThreadTest()
        {
            var sw = new Stopwatch();
            sw.Start();
            var result = BackgroundThread.GetOddNumbersMainThread();
            sw.Stop();
            _testOutputHelper.WriteLine("Elapsed " + sw.ElapsedMilliseconds);
            Assert.All(result, i =>
            {
                Assert.Equal(1, i % 2);
            });
            Assert.Equal(1000000,result.Count);
        }
        
        [Fact]
        public void GetOddNumbersBackgroundThreadTest()
        {
            var sw = new Stopwatch();
            sw.Start();
            var result = BackgroundThread.GetOddNumbersBackgroundThread();
            sw.Stop();
            _testOutputHelper.WriteLine("Elapsed " + sw.ElapsedMilliseconds);
            Assert.All(result, i =>
            {
                Assert.Equal(1, i % 2);
            });
            Assert.Equal(1000000,result.Count);
        }

        [Fact]
        public void ForeverWithMainThreadTest()
        {
            var sw = new Stopwatch();
            sw.Start();
            BackgroundThread.ForeverWithMainThread();
            sw.Stop();
            _testOutputHelper.WriteLine("Elapsed " + sw.ElapsedMilliseconds);
        }
        
        [Fact]
        public void ForeverWithBackgroundThreadTest()
        {
            var sw = new Stopwatch();
            sw.Start();
            BackgroundThread.ForeverWithBackgroundThread();
            sw.Stop();
            _testOutputHelper.WriteLine("Elapsed " + sw.ElapsedMilliseconds);
        }

    
        // public static void ForeverWithBackgroundThread()
        // {
        //     new Thread(() =>
        //     {
        //         while (true)
        //         {
        //             GetFirstMillionOddNumbers();
        //         }
        //     }) {IsBackground = true}.Start();
        //     var sw = new Stopwatch();
        //     sw.Start();
        //     while (sw.Elapsed < TimeSpan.FromSeconds(60))
        //     {
        //         GetFirstMillionOddNumbers();
        //     }
        // }
    }
}