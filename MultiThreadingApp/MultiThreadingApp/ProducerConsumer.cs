using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreadingApp
{
    /// <summary>    
    /// Assignment 1-	Producer Consumer Pattern
    /// Input: List of integers(no fixed limit, over 1K accounts).
    /// Output: 
    /// 1.	Write producer and consumer pattern.
    /// 1.1.	Create configured number of consumers and producers.
    /// 1.2.	Producers will create random account numbers.
    /// 1.3.	Consumer will read numbers and perform below operations.
    /// 1.3.1.	Write a function with integer parameter. This function will mod input number by 99 
    /// 1.3.2.	If number mod 99 = 0 then divide that input number again by mod value (i.e. 0), this will throw divide by 0 exception.
    /// 1.3.3.	Write key value pair of input and mod value and write to file.
    /// 2.	Verify that after processing producer and consume will stop processing without any exception.
    /// </summary>
    public static class ProducerConsumer
    {
        static BlockingCollection<string> objCollection = new BlockingCollection<string>();
        static Mutex mutex = new Mutex();

        public static void StartProducerConsumer()
        {
            Parallel.Invoke(Producer, Consumer);
        }

        private static void Producer()
        {
            Random random = new Random();
            var result = Parallel.For(0, 2, (i) =>
                 {
                     for (int j = 0; j < 10; j++)
                     {
                         objCollection.Add($"ProducerId:{i},AccountId:{ random.Next(1000) }");
                     }
                 });
            while (!result.IsCompleted)
            {
                Thread.Sleep(100);
            }
            objCollection.CompleteAdding();
        }

        private static void Consumer()
        {
            var result = Parallel.For(0, 4, (i) =>
            {
                while (!objCollection.IsCompleted)
                {
                    
                    var item = objCollection.Take();
                    CalculateMod(Convert.ToInt32(GetAccountId(item)));
                    
                    Console.WriteLine($"{item}, ConsumerId:{i}");
                }
            });

            while (!result.IsCompleted)
            {
                Thread.Sleep(100);
            }

        }

        private static void CalculateMod(int num)
        {
           
            int mod = 0;
            mod = num % 99;
            WriteToFile(num, mod);
            

            if (mod==0)
            {
                CallException( num);
            }
        }

        private static string GetAccountId(string input)
        {
            var value =
            input
            .Split(',')
            .Select(
                pair => pair.Split(':'))
            .ToDictionary(
                keyValue => keyValue[0].Trim(),
                keyValue => keyValue[1].Trim())
            ["AccountId"];

            return value;
        }

        private static void WriteToFile(int AccountId, int modResult)
        {
            string file = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "\\DataFile.txt");
            mutex.WaitOne();
            try
            {
                //Utility.WriteData($"AccountId:{AccountId}, Mod99Result:{modResult}");
                using (StreamWriter sw = File.AppendText(file))
                {
                    sw.WriteLine($"AccountId:{AccountId}, Mod99Result:{modResult}");
                }
            }
            finally
            {
                mutex.ReleaseMutex();
            }

        }

        public static Task ThrowDivideByZeroExceptionAsync(int num)
        {
            return Task.Run(() =>
            {
                try
                {
                    var err = num / 0;
                    throw new Exception("Divide by Zero Exception");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            });
        }
        public static async void CallException( int num)
        {
            try
            {
                await ThrowDivideByZeroExceptionAsync(num);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
