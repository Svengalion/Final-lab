using System;
using System.Threading;

namespace TaskSchedulerVsSynchronizationContext
{
    internal class SimpleThreadSynchronizationContext : SynchronizationContext
    {
        public override void Post(SendOrPostCallback d, object? state)
        {
            Console.WriteLine($"Post from SimpleThreadSynchronizationContext in {Thread.CurrentThread.ManagedThreadId}");
            new Thread(() => d.Invoke(state)).Start();
        }

        //public override void Send(SendOrPostCallback d, object? state)
        //{
        //    Console.WriteLine($"Send from SimpleThreadSynchronizationContext in {Thread.CurrentThread.ManagedThreadId}");
        //    d.Invoke(state);
        //}
    }
}