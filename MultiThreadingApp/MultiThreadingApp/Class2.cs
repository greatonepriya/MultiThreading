using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreadingApp
{
    /// <summary>
    /// Assignment 10. Async – How to use async function in lambda expression.  
    /// </summary>
    public static class AsyncInLambdaExpNew
    {

        static async Task ExampleMethodAsync()
        {

            Func<Task> AsyncLambda = async () =>
            {
                Console.WriteLine("Async Call");
                await Task.Delay(100);
                for (int i = 0; i < 10; i++)
                {
                    Thread.Sleep(1000);
                    Console.WriteLine($"Iteration # {i + 1} completed");
                }
                Console.WriteLine("Async Call End");
            };

             await RunMe(AsyncLambda);
            //await Task.Run(AsyncLambda);
        }


        static Task RunMe(Func<Task> Runner)
        {
            Console.WriteLine("RunMe Call ");
            if (Runner != null)
                return Task.Run(Runner);

            return null;
        }

        public static void AsyncInLambdaExpression()
        {
            ExampleMethodAsync().Wait();
        }

    }
}
