using System;
using System.Diagnostics;
using System.Threading;
using ParallelProgrammingSamples;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    public class LockingAndThreadSafetyTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public LockingAndThreadSafetyTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void GetRandomNumberTest()
        {
            for (var i = 0; i < 1000; i++)
            {
                _testOutputHelper.WriteLine(LockingAndThreadSafety.GetRandomNumber().ToString());
            }

            Assert.True(true);
        }

        [Fact]
        public void GetRandomNumberWithInterlockedTest()
        {
            for (var i = 0; i < 10; i++)
            {
                _testOutputHelper.WriteLine(LockingAndThreadSafety.GetRandomNumberWithInterlocked().ToString());
            }

            Assert.True(true);
        }

        [Fact]
        public void GetRandomNumberFiniteCountTest()
        {
            for (var i = 0; i < 10; i++)
            {
                _testOutputHelper.WriteLine(LockingAndThreadSafety.GetRandomNumber1().ToString());
            }

            Assert.True(true);
        }

        [Fact]
        public void GetRandomNumberFiniteCountWithInterlockedTest()
        {
            for (var i = 0; i < 10; i++)
            {
                _testOutputHelper.WriteLine(LockingAndThreadSafety.GetRandomNumberWithInterlocked1().ToString());
            }

            Assert.True(true);
        }


        [Fact]
        public void TryDeadlockTest()
        {
            var sw = new Stopwatch();
            for (var i = 0; i < 10; i++)
            {
                sw.Start();
                _ = LockingAndThreadSafety.TryDeadlock();
                sw.Stop();
                _testOutputHelper.WriteLine(sw.Elapsed >= TimeSpan.FromSeconds(4) ? "Deadlock" : "Did not deadlock");
                Thread.Sleep(2000);
                Assert.True(true);
            }
        }

        [Fact]
        public void LockRaceTest()
        {
            for (var i = 0; i < 100; i++)
            {
                var result = LockingAndThreadSafety.TryDeadlock();
                _testOutputHelper.WriteLine(result.ToString());
                Assert.True(true);
            }
        }
    }
}