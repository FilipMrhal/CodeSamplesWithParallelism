using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ParallelProgrammingSamples;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    public class CacheTests
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private static readonly int[] ArrayLenght = { 2, 4, 8, 10, 100, 1000, 10000};

        public CacheTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }
        
        private void BaseTest(Func<int[], int[]> cache, string name, bool tooSlow = false)
        {
            var sw = new Stopwatch();
            foreach (var length in tooSlow ? ArrayLenght.Take(8) : ArrayLenght)
            {
                var randomArray = Utilities.GenerateRandomArray(length);
                sw.Start();
                var result = cache.Invoke(randomArray);
                sw.Stop();
                _testOutputHelper.WriteLine($"Elapsed {name} {sw.ElapsedMilliseconds}");
                Assert.All(randomArray, i => Assert.Single(result, x => x == i));
            }
        }

        private void DuplicateKeysTest(Func<int[], int[]> cache, string name)
        {
            var sw = new Stopwatch();
            sw.Start();
            var result = cache.Invoke(new []{1,1,1,1,1,1,1}); //We try to add similar key more than once.
            sw.Stop();
            _testOutputHelper.WriteLine($"Elapsed {name} {sw.ElapsedMilliseconds}");
            Assert.Single(result);
        }
        
        private async Task DuplicateKeysTestAsync(Func<int[], Task<int[]>> cache, string name)
        {
            var sw = new Stopwatch();
            sw.Start();
            var result =await cache.Invoke(new []{1,1,1,1,1,1,1}); //We try to add similar key more than once.
            sw.Stop();
            _testOutputHelper.WriteLine($"Elapsed {name} {sw.ElapsedMilliseconds}");
            Assert.Single(result);
        }
        
        private async Task BaseTestAsync(Func<int[], Task<int[]>> cache, string name, bool tooSlow = false)
        {
            var sw = new Stopwatch();
            sw.Start();
            var result = await cache.Invoke(new []{1,1,1,1,1,1,1,}); //We try to add similar key more than once.
            sw.Stop();
            _testOutputHelper.WriteLine($"Elapsed {name} {sw.ElapsedMilliseconds}");
            Assert.Single(result);
            
            foreach (var length in tooSlow ? ArrayLenght.Take(8) : ArrayLenght)
            {
                var randomArray = Utilities.GenerateRandomArray(length);
                sw.Start();
                result = await cache.Invoke(randomArray);
                sw.Stop();
                _testOutputHelper.WriteLine($"Elapsed {name} {sw.ElapsedMilliseconds}");
                Assert.All(randomArray, i => Assert.Single(result, x => x == i));
            }
        }
        
        [Fact]
        public async Task SequentialCacheUnderParallelCalls1() => await BaseTestAsync(new Cache().CacheTester1, "Cache");
        
        [Fact]
        public void SequentialCacheUnderParallelCalls2() => BaseTest(new Cache().CacheTester2, "Cache");
        
        [Fact]
        public async Task SequentialCacheUnderParallelCallsDuplicateKeys1() => await DuplicateKeysTestAsync(new Cache().CacheTester1, "Cache");
        
        [Fact]
        public void SequentialCacheUnderParallelCallsDuplicateKeys2() => DuplicateKeysTest(new Cache().CacheTester2, "Cache");
        
        
        
        [Fact]
        public async Task CacheWithLockingUnderParallelCalls1() => await BaseTestAsync(new CacheWithLocking().CacheTester1, "Cache with locking ");
        
        [Fact]
        public void CacheWithLockingUnderParallelCalls2() => BaseTest(new CacheWithLocking().CacheTester2, "Cache with locking ");
        
        [Fact]
        public async Task CacheWithLockingUnderParallelCallsDuplicateKeys1() => await DuplicateKeysTestAsync(new CacheWithLocking().CacheTester1, "Cache with locking ");
        
        [Fact]
        public void CacheWithLockingUnderParallelCallsDuplicateKeys2() => DuplicateKeysTest(new CacheWithLocking().CacheTester2, "Cache with locking ");
        
        
        
        [Fact]
        public async Task CacheWithConcurrencyUnderParallelCalls1() => await BaseTestAsync(new CacheWithConcurrency().CacheTester1, "Cache with concurrency");
        
        [Fact]
        public void CacheWithConcurrencyUnderParallelCalls2() => BaseTest(new CacheWithConcurrency().CacheTester2, "Cache with concurrency");
        
        [Fact]
        public async Task CacheWithConcurrencyUnderParallelCallsDuplicateKeys1() => await DuplicateKeysTestAsync(new CacheWithConcurrency().CacheTester1, "Cache with concurrency");
        
        [Fact]
        public void CacheWithConcurrencyUnderParallelCallsDuplicateKeys2() => DuplicateKeysTest(new CacheWithConcurrency().CacheTester2, "Cache with concurrency");
    }
}