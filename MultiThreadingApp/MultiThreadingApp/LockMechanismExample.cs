using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreadingApp
{
    public static class LockMechanismExample
    {
        //readonly static string file = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "\\DataFile.txt");
        readonly static Random random = new Random();

        /// <summary> Assignment 5- Mutex.

        ///1. Write random numbers to file in parallel for loop.

        ///2. Use Mutex to make file thread safe and allow only one thread to write file.
        /// </summary>
        public static void MutexExample()
        {
            Mutex mutex = new Mutex();

            string file = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "\\DataFile.txt");
            Parallel.For(0, 5, (i) =>
            {

                var result = random.Next(1000);
                mutex.WaitOne();
                try
                {
                    using (StreamWriter sw = File.AppendText(file))
                    {
                        sw.WriteLine(result);
                    }
                }
                finally
                {
                    mutex.ReleaseMutex();
                }

            });
        }

        /// <summary> Assignment 7-Semaphore

        ///1. Write random numbers to C# list in parallel for loop.

        ///2. Use semaphore to allow 2 threads to add item in the list.
        /// </summary>
        public static void SemaphoreExample()
        {
            Semaphore semaphore = new Semaphore(2, 2);

            BlockingCollection<string> templist = new BlockingCollection<string>();

            Parallel.For(0, 5, (i) =>
            {

                var result = random.Next(1000);
                semaphore.WaitOne();
                try
                {
                    templist.Add($"TaskId:{Task.CurrentId}, num:{result }"); ;
                    //templist.Add(result);
                }
                finally
                {
                    semaphore.Release();
                }

            });

            Console.WriteLine("\nThe numbers in the list are : ");
            foreach (var lstnum in templist)
            {
                Console.Write(lstnum + " ");
            }

        }

        /// <summary> Assignment 6-  Auto Reset Event and Manual Reset Event

        ///1. Write random numbers to C# list in parallel for loop.

        ///2. Use AutoResetEvent or manual reset event to make list thread safe and allow only one thread to write file.

        /// </summary>

        static BlockingCollection<string> list = new BlockingCollection<string>();
        static ManualResetEvent manualReset = new ManualResetEvent(false);
        public static void ManualResetEventExample()
        {

            try
            {
                Parallel.For(0, 5, (i) =>
              {
                  manualReset.Set();
                  WriteNumToList(random.Next(1000));

              });
            }
            finally
            {
                manualReset.Reset();
            }


            Console.WriteLine("\nThe numbers in the list are : ");
            foreach (var lstnum in list)
            {
                Console.Write(lstnum + " ");
            }
        }
        static void WriteNumToList(int num)
        {
            list.Add($"TaskId:{Task.CurrentId}, num:{num }");
            manualReset.WaitOne();

        }

        /// <summary> Assignment 8- Async Await and exception handling.

        ///1. Write a long running Aync function, function will do some long running work and will throw exception on specific condition.

        ///2. Handle exception check how caller can capture exception.

        /// </summary>

        public static void ExceptionHandlingtExample()
        {
        }
    }
}

