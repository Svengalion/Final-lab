using static System.Console;
using System.Threading.Tasks;
using System.Threading;

namespace TaskSchedulerVsSynchronizationContext
{
    internal class Program
    {
        //показывает приколюхи
        //static Singleton
        static Program() => SynchronizationContext.SetSynchronizationContext(new SimpleThreadPoolSynchronizationContext()); //программа будет выполняться в контексте синхронизации (самописном)
        
        private static void Work(object? _) //просто функция
        {
            Thread.Sleep(1000);
            WriteLine($"Method Work (task №{Task.CurrentId}) is completed in {Thread.CurrentThread.ManagedThreadId}");
        }

        private static async Task WorkAsync(object? _)//та же функция, но асинхронная
        {
            Task task = Task.Delay(1000);
            await task;//отправляет task в работу
            //await task.ConfigureAwait(false); 
            WriteLine($"Method WorkAsync (task №{task.Id}) is completed in {Thread.CurrentThread.ManagedThreadId}");
        }

        #region Singleton
        private static MinThreadTaskScheduler? _scheduler;
        private static MinThreadTaskScheduler GetScheduler => 
            _scheduler ??= new MinThreadTaskScheduler();

        #endregion

        static async Task Main()
        {
            WriteLine($"Method Main is started in {Thread.CurrentThread.ManagedThreadId}");

            Task task = new Task(Work!, null); //создает новый поток выполняющий Work
            //task.Start();
            //task.Wait(); //в основном потоке будет ожидать
            //task.Start(GetScheduler); //запускает с помощью планировщика и выбирает дефолтный (если не изменен, то это основной поток программы) параллельности не будет, всче находится в основном потоке
            //task.Start(TaskScheduler.FromCurrentSynchronizationContext()); // запустит поток в нынешнем контексте синхронизации
            //task.Start(TaskScheduler.Current); // запустит поток в выполняющемся потоке
            task = Task.Factory.StartNew(() =>
            {
                WriteLine($"Current TaskScheduler - {TaskScheduler.Current.GetType().FullName}"); //возвращает название типа текущего планировщика задач
                Task.Run(() => WriteLine($"\tThe nested-task  (task №{Task.CurrentId}) is completed in {Thread.CurrentThread.ManagedThreadId}"));//запускает новый task 
                Task.Factory.StartNew(() => WriteLine($"\tThe child-task (task №{Task.CurrentId}) is completed in {Thread.CurrentThread.ManagedThreadId}"),
                    TaskCreationOptions.AttachedToParent/*передает параметры создания из внешнего factory в этот*/); //создает и запускает factory 
                WriteLine($"Task \"task\" (task №{Task.CurrentId}) is completed in {Thread.CurrentThread.ManagedThreadId}");
            }, CancellationToken.None, TaskCreationOptions.None, GetScheduler); // HideScheduler
            task.Wait();
            await WorkAsync(null);
            await WorkAsync(null).ConfigureAwait(false);

            ReadKey();
        }
    }
}
