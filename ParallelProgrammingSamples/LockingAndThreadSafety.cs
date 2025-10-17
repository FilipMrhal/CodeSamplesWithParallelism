using System;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelProgrammingSamples
{
    public static class LockingAndThreadSafety
    {
        private static int _start = 0;

        public static int GetRandomNumber()
        {
            var cts = new CancellationTokenSource();
            Task.Run(() => _start++, cts.Token);
            Task.Run(() => _start--, cts.Token);
            cts.CancelAfter(TimeSpan.FromSeconds(8));
            return _start;
        }

        public static int GetRandomNumberWithInterlocked()
        {
            var cts = new CancellationTokenSource();
            Task.Run(() => Interlocked.Increment(ref _start), cts.Token);
            Task.Run(() => Interlocked.Decrement(ref _start), cts.Token);
            cts.CancelAfter(TimeSpan.FromSeconds(8));
            return _start;
        }

        public static int GetRandomNumber1()
        {
            var cts = new CancellationTokenSource();
            var t1 = Task.Run(() =>
            {
                for (var i = 0; i < 1000; i++)
                    _start++;
            }, cts.Token);
            var t2 = Task.Run(() =>
            {
                for (var i = 0; i < 1000; i++)
                    _start--;
            }, cts.Token);
            Task.WaitAll(t1, t2);
            return _start;
        }

        public static int GetRandomNumberWithInterlocked1()
        {
            var cts = new CancellationTokenSource();
            var t1 = Task.Run(() =>
            {
                for (var i = 0; i < 1000; i++)
                    Interlocked.Increment(ref _start);
            }, cts.Token);
            var t2 = Task.Run(() =>
            {
                for (var i = 0; i < 1000; i++)
                    Interlocked.Decrement(ref _start);
            }, cts.Token);
            Task.WaitAll(t1, t2);
            return _start;
        }

        #region Dead locking

        public static int TryDeadlock()
        {
            var lockA = new object();
            var lockB = new object();
            var cts = new CancellationTokenSource();
            var value = 0;
            var tA = new Task(() =>
            {
                lock (lockA)
                {
                    Task.Delay(5, cts.Token);
                    lock (lockB)
                    {
                        value = 3;
                    }
                }
            }, cts.Token);
            var tB = new Task(() =>
            {
                lock (lockB)
                {
                    Task.Delay(5, cts.Token);
                    lock (lockA)
                    {
                        value = 5;
                    }
                }
            }, cts.Token);
            tA.Start();
            tB.Start();
            cts.CancelAfter(TimeSpan.FromSeconds(30));
            Task.WaitAll([tA, tB], TimeSpan.FromSeconds(30));
            return value;
        }

        #endregion

        #region Lock race

        private static readonly object _locker = new object();
        private static bool go;
        private static int number = 0;

        public static int Go()
        {
            new Thread(IncrementNumber).Start();

            for (var i = 0; i < 5; i++)
            {
                lock (_locker)
                {
                    go = true;
                    Monitor.Pulse(_locker);
                }
            }

            return number;
        }

        private static void IncrementNumber()
        {
            for (var i = 0; i < 5; i++)
            {
                lock (_locker)
                {
                    while (!go)
                        Monitor.Wait(_locker);
                    go = false;
                }

                Interlocked.Increment(ref number);
            }
        }

        #endregion

        #region Locking

        private static readonly object _locking = new object();

        public static void Lock1()
        {
            int val1 = 200;
            int val2 = 0;
            lock (_locking)
            {
                if (val2 != 0)
                    Console.WriteLine(val1 / val2);
                val2 = 0;
            }
        }

        public static void Lock2()
        {
            int val1 = 200;
            int val2 = 0;
            bool lockWasTaken = false;
            Monitor.Enter(_locking, ref lockWasTaken);
            try
            {
                if (val2 != 0)
                    Console.WriteLine(val1 / val2);
                val2 = 0;
            }
            finally
            {
                if (lockWasTaken)
                    Monitor.Exit(_locking);
            }
        }
        
        public static string CanILockWithNullable()
        {
            var lockA = new Nullable<int>(0);
            var cts = new CancellationTokenSource();
            //Can I lock lockA increment with lockA?
            return string.Empty;
        }

        #endregion
    }
}