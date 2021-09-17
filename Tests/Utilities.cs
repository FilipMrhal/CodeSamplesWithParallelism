using System;
using System.Linq;

namespace Tests
{
    public static class Utilities
    {
        public static int[] GenerateRandomArray(int length)
        {
            const int min = 0;
            const int max = 1000;

            var randNum = new Random();
            return Enumerable
                .Repeat(0, length)
                .Select(i => randNum.Next(min, max))
                .ToArray();
        }
    }
}