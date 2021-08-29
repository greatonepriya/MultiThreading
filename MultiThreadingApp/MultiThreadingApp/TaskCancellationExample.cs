using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreadingApp
{
    /// <summary>
    /// Assignment 4. Cancellation Token    
    /// </summary>
    public static class TaskCancellationExample
    {
       
        public static void TaskCancellation()
        {

            CancellationTokenSource source = new CancellationTokenSource();

            var task = Task.Run(() => DoWork(source.Token), source.Token);

            Thread.Sleep(2500);
            source.Cancel();
            Console.WriteLine("\nTask cancellation requested.");

            try
            {
                task.Wait();
            }
            catch (AggregateException ae)
            {

                if (ae.InnerException is TaskCanceledException)
                    Console.WriteLine("Task cancelled exception detected");
                else
                    throw;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                source.Dispose();
            }

           
        }

        private static void DoWork(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine("Cancelled work before start");
                cancellationToken.ThrowIfCancellationRequested();
            }

            for (int i = 0; i < 10000; i++)
            {
                Thread.Sleep(1000);
                if (cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine($"Cancelled on iteration # {i + 1}");

                    cancellationToken.ThrowIfCancellationRequested();
                }
                Console.WriteLine($"Iteration # {i + 1} completed");
            }
        }

    }
}
