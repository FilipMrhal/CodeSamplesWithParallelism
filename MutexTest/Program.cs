using System;
using System.Threading;

namespace MutexTest
{
    class Program
    {
        static Mutex _mutex = new Mutex(false, "Mutex testovaci appka");

        static void Main(string[] args)
        {
            if (!_mutex.WaitOne(TimeSpan.FromSeconds(2), false))
            {
                Console.WriteLine("Application cannot run because another instance has already started!");
                Console.ReadLine();
                return;
            }

            try
            {
                Console.WriteLine("Application runs.");
                Console.ReadLine();
            }
            finally
            {
                _mutex.ReleaseMutex();
            }
        }
    }
}