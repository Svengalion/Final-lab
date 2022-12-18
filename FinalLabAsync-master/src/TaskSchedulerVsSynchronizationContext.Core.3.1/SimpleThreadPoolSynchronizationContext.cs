﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskSchedulerVsSynchronizationContext
{
    internal class SimpleThreadPoolSynchronizationContext : SynchronizationContext
    {
        public override void Post(SendOrPostCallback d, object? state)
        {
            Console.WriteLine($"Post from SimpleThreadPoolSynchronizationContext in {Thread.CurrentThread.ManagedThreadId} [task №{Task.CurrentId}]");
            //Task.Factory.StartNew(() => d.Invoke(state));
            ThreadPool.QueueUserWorkItem(_ => d.Invoke(state));
        }

        //public override void Send(SendOrPostCallback d, object? state)
        //{
        //    Console.WriteLine($"Send from SimpleThreadPoolSynchronizationContext in {Thread.CurrentThread.ManagedThreadId} [task №{Task.CurrentId}]");
        //    d.Invoke(state);
        //}
    }
}