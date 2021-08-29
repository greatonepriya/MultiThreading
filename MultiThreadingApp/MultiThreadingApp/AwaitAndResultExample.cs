using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreadingApp
{
    /// <summary>
    /// Assignment 9. Async – Await vs Result 
    /// </summary>
    public static class AwaitAndResultExample
    {
        public static void AwaitAndResultCaller()
        {
            AwaitCall().Wait();

            ResultCall();
        }
        static void ResultCall()
        {
            Console.WriteLine("ResultCall start");
            Task<int> task = new Task<int>(obj =>
            {
                int total = 0;
                int max = (int)obj;
                for (int i = 0; i < max; i++)
                {
                    total += i;
                }
                return total;
            }, 300);

            task.Start();
            int result = Convert.ToInt32(task.Result);
            Console.WriteLine($"ResultCall End output: {result}");
        }

        

        static async Task AwaitCall()
        {
            Console.WriteLine("AwaitCall start");
            Func<int> function = new Func<int>(() =>
            {
                int count = 0;
                for (int i = 0; i < 10; i++)
                {
                    Thread.Sleep(1000);
                    Console.WriteLine($"Iteration # {i + 1} completed");
                    count++;
                }
                return count;
            });
            int res = await Task.Run<int>(function);
            Console.WriteLine($"AwaitCall End output: {res}");
        }

        
    }
}
