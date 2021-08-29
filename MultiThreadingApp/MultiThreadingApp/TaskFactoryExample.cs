using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreadingApp
{
    public static class TaskFactoryExample
    {
        /// <summary>    
        /// Assignment 2-	Thread Factory
        /// Input: List of integers(no fixed limit, over 1K accounts).
        /// Output: 1. Process provided input list (items more than 1K).
        ///2. Create configured number of threads(example create 5 threads or based on available cores on your system)
        ///3. Execute above threads parallelly using parallel foreach or parallel for loop.
        ///4. Each thread will execute parallelly but will process 2 accounts at time(2 in sequence).
        ///5. That means 5 threads in parallel and each thread will process 2 accounts sequentially.
        ///6. Account processing logic:
        ///6.1. Write a function with integer parameter. This function will mod input number by 99
        ///6.2. If number mod 99 = 0 then divide that input number again by mod value (i.e. 0), this will throw divide by 0 exception.
        ///6.3. Create key value pair of input and mod value and return output back to caller.
        ///6.3.1. Collection should be thread safe or use thread synchronization technic.
        ///7. In Main function (In parallel loop), harvest result and check how many accounts have exception and how many processed successfully.
        /// </summary>
        /// 
        static Mutex mutex = new Mutex();
        readonly static Random random = new Random();
        static int count = 0;
        public static void CreateTaskFactory(int numOfTasks, int numberOfRecords)
        {
            
            Task[] tasks = new Task[numOfTasks];
            for (int i = 0; count< numberOfRecords; i++)
            {
                var result = Parallel.For(0, numOfTasks, (j) =>
                {
                    tasks[j] = Task.Factory.StartNew(() =>
                    {

                        if (i == 0)
                        {
                            count++;
                            CalculateMod(random.Next(1000));
                        }
                        else
                        {
                            if (i % 2 == 0)
                            {
                                count++;
                                CalculateMod(random.Next(1000));
                            }
                            else
                            {
                                count++;
                                CalculateMod(random.Next(1000));
                                 
                            }
                        }

                    });

                    Thread.Sleep(1000);

                    var taskFactory = Task.Factory.ContinueWhenAll(tasks, completedTasks =>
                    {
                        for (int i = 0; i < completedTasks.Length; i++)
                        {
                            if (completedTasks[i].Status.Equals(TaskStatus.RanToCompletion))
                            {
                                Console.WriteLine($"Task Completed, TaskId:{i}");
                            }
                            else if (completedTasks[i].Status.Equals(TaskStatus.Faulted))
                            {
                                Console.WriteLine($"Task Faulted, Ex:{completedTasks[i].Exception.Message} TaskId:{i}");
                            }
                            else
                            {
                                Console.WriteLine($"Task Status:{completedTasks[i].Status}, TaskId:{i}");
                            }
                        }
                    });

                    taskFactory.Wait();
                });
            }

           
        }
        private static void CalculateMod(int num)
        {

            int mod = 0;
            mod = num % 99;
          
            if (mod == 0)
            {
                var result = ThrowDivideByZeroExceptionAsync(num);
            }
            WriteToFile(num, mod);
        }

        private static void WriteToFile(int AccountId, int modResult)
        {
            string file = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "\\DataFile.txt");
            mutex.WaitOne();
            try
            {
                
                using (StreamWriter sw = File.AppendText(file))
                {
                    sw.WriteLine($"TaskId:{Task.CurrentId}, Number:{AccountId}, ModResult:{modResult}");
                }
            }
            finally
            {
                mutex.ReleaseMutex();
            }

        }

        public static async Task<int> ThrowDivideByZeroExceptionAsync(int num)
        {
            int err = num / 0;
            return err;
        }

    }
}
