using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace MainThreadTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        GetFirstMillionOddNumbers();
                    }
                    finally
                    {
                        Console.WriteLine("Ended correctly");
                        Console.ReadLine();
                    }
                }
            }).Start();
            var sw = new Stopwatch();
            sw.Start();
            while (sw.Elapsed < TimeSpan.FromSeconds(20))
            {
                Thread.Sleep(32);
            }
            Debug.WriteLine("EXITING");
            Environment.Exit(0);
        }

        private static void GetFirstMillionOddNumbers()
        {
            var memory = new List<int>();
            var number = 0;
            while (true)
            {
                if (number % 2 == 1)
                {
                    memory.Add(number);
                    Debug.WriteLine("Still here.");
                    Debug.WriteLine(number);
                }

                number++;
            }
        }
    }
}