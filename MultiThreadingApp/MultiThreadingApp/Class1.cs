using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreadingApp
{
    /// <summary>
    /// Assignment 8. Async Await and exception handling.
    /// </summary>
    public static class Class1
    {
        static BlockingCollection<string> objCollection = new BlockingCollection<string>();

        static async Task<Task> ExceptionInAsync()
        {

            Task tasks = null;
            //try
            //{
                var task1 = Task.Run(() =>
                {
                    for (int i = 0; i < 10000; i++)
                    {
                        Thread.Sleep(1000);
                        if (i > 5)
                        {
                            throw new IndexOutOfRangeException("IndexOutOfRangeException is thrown.");
                        }
                        objCollection.Add($"Task 1 Item # {i + 1}");
                    }
                });
                var task2 = Task.Run(() =>
                {
                    for (int i = 0; i < 10000; i++)
                    {
                        Thread.Sleep(1000);
                        if (i > 5)
                        {
                            throw new ArithmeticException("ArithmeticException is thrown.");
                        }
                        objCollection.Add($"Task 2 Item # {i + 1}");
                    }
                });

                tasks = Task.WhenAll(task1, task2);

                await tasks;
                //tasks.Wait();
                objCollection.CompleteAdding();
            return tasks;
            //}
            //catch
            //{
            //    AggregateException aggregateException = tasks.Exception;
            //    foreach (var e in aggregateException.InnerExceptions)
            //    {
            //        Console.WriteLine("Exception Occured: " + e.GetType().ToString());
            //    }
            //}
        }

        public static async void CallException()
        {
            Task t = ExceptionInAsync();
            try
            {


                t.Wait();
            }
            catch
            {
                AggregateException aggregateException = t.Exception;
                foreach (var e in aggregateException.InnerExceptions)
                {
                    Console.WriteLine("Exception Occured: " + e.GetType().ToString());
                }
            }
            //await ExceptionInAsync();
            while (!objCollection.IsCompleted)
            {

                var item = objCollection.Take();


                Console.WriteLine($"{item}");
            }
        }




    }


}
