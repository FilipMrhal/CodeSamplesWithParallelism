using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;

namespace ParallelProgrammingSamples
{
    public class Synchronization
    {
        #region Blocking

        #region Sleep

        public void Sleep()
        {
            //Frees CPU
            Thread.Sleep(0); //Sleep one time-slice
            Thread.Sleep(1000); 
            Thread.Sleep(TimeSpan.FromMinutes(1));
            Thread.Sleep(Timeout.Infinite); //Lze vypnout pres Thread.Interrupt
        }

        #endregion

        #region SpinWait

        public void SpinWait()
        {
            Thread.SpinWait(1000); 
        }

        #endregion

        #region Spinning

        public void Spinning()
        {
            bool proceed = true;
            while (proceed)
            {
                Thread.Sleep(20);
            }
        }

        #endregion

        #region Join

        public void Join()
        {
            var t = new Thread(() => Console.ReadLine());
            t.Start();
            t.Join(); //Wait until completion.
            Console.WriteLine("done");
        }

        #endregion
        
        #region Semaphore

        private readonly Semaphore _semaphore = new Semaphore(3, 3);

        public void TestSemaphore()
        {
            for (var i = 0; i < 1000; i++) new Thread(Work).Start();
        }
        
        private void Work()
        {
            while (true)
            {
                _semaphore.WaitOne();
                Thread.Sleep(TimeSpan.FromSeconds(3));
                _semaphore.Release();
            }
        }

        #endregion

        #endregion

        #region Signalization

        #region EventWaitHandle

        #region AutoResetEvent

        private EventWaitHandle wh1 = new AutoResetEvent(false);
        private EventWaitHandle wh2 = new EventWaitHandle(false, EventResetMode.AutoReset);
        //Manual just needs to be Reset manually.
        //Adding a unique name will allow the wait handle to work cross process.

        public void AutoResetEventTest()
        {
            new Thread(Waiter).Start();
            Thread.Sleep(5000);
            wh1.Set();
            wh1.Close();
        }
        
        private void Waiter()
        {
            Debug.WriteLine("Waiting...");
            wh1.WaitOne();
            Console.WriteLine("Released");
        }

        //TODO Add simple background worker sample.
        //TODO Add producer/consumer sample.

        public void SignalAndWaitTest()
        {
            WaitHandle.SignalAndWait(wh1, wh2);
            WaitHandle.SignalAndWait(wh2, wh1);
        }

        #endregion

        #endregion
        
        #endregion
    }
}