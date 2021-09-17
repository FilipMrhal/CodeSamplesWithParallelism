using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace ParallelProgrammingSamples
{
    public static class BackgroundThread
    {
        public static List<int> GetOddNumbersWithoutThreading() =>
            GetFirstMillionOddNumbers();

        public static List<int> GetOddNumbersMainThread()
        {
            var result = new List<int>();
            var worker = new Thread(() => { result = GetFirstMillionOddNumbers(); });
            worker.Start();
            worker.Join();
            return result;
        }

        public static List<int> GetOddNumbersBackgroundThread()
        {
            var result = new List<int>();
            var worker = new Thread(() => { result = GetFirstMillionOddNumbers(); }) { IsBackground = true };
            worker.Start();
            worker.Join();
            return result;
        }

        public static void ForeverWithMainThread()
        {
            new Thread(() =>
            {
                while (true)
                {
                    GetFirstMillionOddNumbers();
                }
            }).Start();
            var sw = new Stopwatch();
            sw.Start();
            while (sw.Elapsed < TimeSpan.FromSeconds(10))
            {
                GetFirstMillionOddNumbers();
            }
        }
        
        public static void ForeverWithBackgroundThread()
        {
            new Thread(() =>
            {
                while (true)
                {
                    GetFirstMillionOddNumbers();
                }
            }) {IsBackground = true}.Start();
            var sw = new Stopwatch();
            sw.Start();
            while (sw.Elapsed < TimeSpan.FromSeconds(10))
            {
                GetFirstMillionOddNumbers();
            }
        }

        private static List<int> GetFirstMillionOddNumbers()
        {
            var result = new List<int>();
            var count = 0;
            var number = 0;
            while (count < 1000000)
            {
                if (number % 2 == 1)
                {
                    result.Add(number);
                    count++;
                }
                number++;
            }

            return result;
        }
    }
}