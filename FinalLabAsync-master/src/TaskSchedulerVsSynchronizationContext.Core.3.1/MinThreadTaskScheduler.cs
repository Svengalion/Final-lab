using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
//это планировщик
namespace TaskSchedulerVsSynchronizationContext
{
    internal class MinThreadTaskScheduler : TaskScheduler
    {
        protected override IEnumerable<Task>? GetScheduledTasks() =>
            Enumerable.Empty<Task>();
        //возвращает запланированные таски
        protected override void QueueTask(Task task) //добавляет задачу в очередь планировщика
        {
            Console.WriteLine($"QueueTask from MinThreadTaskScheduler in {Thread.CurrentThread.ManagedThreadId} [task №{task.Id}]");
            new Thread(() => TryExecuteTask(task)).Start();
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued) //пытается выполнить выданную задачу с максимальным приоритетом
        {
            Console.WriteLine($"TryExecuteTaskInline from MinThreadTaskScheduler in {Thread.CurrentThread.ManagedThreadId} [task №{task.Id}]");
            return TryExecuteTask(task); //пропуск без очереди
        }

        // protected override bool TryDequeue(Task task)

        // по умолчанию int.MaxValue
        // public override int MaximumConcurrencyLevel { get; } 
    }
}