using TaskSchedulerVsSynchronizationContext;

SynchronizationContext.SetSynchronizationContext(new SimpleThreadPoolSynchronizationContext());

void Work(object? _)
{
    Thread.Sleep(1000);
    WriteLine($"Method Work (task №{Task.CurrentId}) is completed in {Thread.CurrentThread.ManagedThreadId}");
}

async Task WorkAsync(object? _)
{
    Task task = Task.Delay(1000);
    await task;
    //await task.ConfigureAwait(false);
    WriteLine($"Method WorkAsync (task №{task.Id}) is completed in {Thread.CurrentThread.ManagedThreadId}");
}

WriteLine($"Method Main is started in {Thread.CurrentThread.ManagedThreadId}");

#region Singleton
TaskScheduler? scheduler = null;
TaskScheduler GetScheduler() =>
    scheduler ??= new MinThreadTaskScheduler();
#endregion

Task task = new Task(Work!, null);
//task.Start();
//task.Wait();
task.Start(GetScheduler());
//task.Start(TaskScheduler.FromCurrentSynchronizationContext());
//task.Start(TaskScheduler.Current);
//task = Task.Factory.StartNew(() =>
//{
//    WriteLine($"Current TaskScheduler - {TaskScheduler.Current.GetType().FullName}");
//    Task.Run(() => WriteLine($"\tThe nested-task  (task №{Task.CurrentId}) is completed in {Thread.CurrentThread.ManagedThreadId}"));
//    Task.Factory.StartNew(() => WriteLine($"\tThe child-task (task №{Task.CurrentId}) is completed in {Thread.CurrentThread.ManagedThreadId}"),
//        TaskCreationOptions.AttachedToParent);
//    WriteLine($"Task \"task\" (task №{Task.CurrentId}) is completed in {Thread.CurrentThread.ManagedThreadId}");
//}, CancellationToken.None, TaskCreationOptions.None, GetScheduler); // HideScheduler
//task.Wait();
//await WorkAsync(null);
//await WorkAsync(null).ConfigureAwait(false);

ReadKey();