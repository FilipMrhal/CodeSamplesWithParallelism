using System;
using System.Diagnostics;
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
            for (var i = 0; i < 10; i++)
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
                var result = LockingAndThreadSafety.TryDeadlock();
                sw.Stop();
                if (sw.Elapsed >= TimeSpan.FromSeconds(4))
                {
                    _testOutputHelper.WriteLine("Deadlock");
                    Assert.True(true);
                }
                else
                {
                    _testOutputHelper.WriteLine("Did not deadlock");
                    Assert.True(true);
                }
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