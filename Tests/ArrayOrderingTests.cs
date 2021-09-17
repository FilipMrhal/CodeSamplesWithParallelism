using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ParallelProgrammingSamples;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    public class ArrayOrderingTests
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private static readonly int[] ArrayLenght = { 2, 4, 8, 10, 100, 1000, 10000, 100000, 1000000, 10000000, 100000000 };

        public ArrayOrderingTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void Test1() => BaseTest(ArrayOrdering.SerialQuickSort, "QuickSort - sequential");

        [Fact]
        public async void Test2() => await BaseTestAsync(ArrayOrdering.ParallelQuickSort, "QuickSort - Parallel");

        [Fact]
        public void Test3() => BaseTest(ArrayOrdering.ShakerSort, "ShakerSort", true);

        [Fact]
        public void Test4() => BaseTest(ArrayOrdering.MergeSort, "Merge Sort", true);

        [Fact]
        public void Test6() => BaseTest(ArrayOrdering.SortSequential, "LINQ");

        [Fact]
        public void Test7() => BaseTest(ArrayOrdering.SortParallel, "PLINQ");

        private void BaseTest(Func<int[], int[]> sorting, string name, bool tooSlow = false)
        {
            foreach (var length in tooSlow ? ArrayLenght.Take(8) : ArrayLenght)
            {
                var randomArray = Utilities.GenerateRandomArray(length);
                var sw = new Stopwatch();
                sw.Start();
                var result = sorting.Invoke(randomArray);
                sw.Stop();
                _testOutputHelper.WriteLine($"Elapsed {name} {sw.ElapsedMilliseconds}");
                for (var i = 0; i < result.Length - 1; i++)
                    Assert.True(result[i] <= result[i + 1]);
            }
        }

        private async Task BaseTestAsync(Func<int[], Task<int[]>> sorting, string name)
        {
            foreach (var length in ArrayLenght)
            {
                var randomArray = Utilities.GenerateRandomArray(length);
                var sw = new Stopwatch();
                sw.Start();
                var result = await sorting.Invoke(randomArray);
                sw.Stop();
                _testOutputHelper.WriteLine($"Elapsed {name} {sw.ElapsedMilliseconds}");
                for (var i = 0; i < result.Length - 1; i++)
                    Assert.True(result[i] <= result[i + 1]);
            }
        }
    }
}