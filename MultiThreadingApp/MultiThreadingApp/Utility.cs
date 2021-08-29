using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreadingApp
{
    public static class Utility
    {
        readonly static string file = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "\\DataFile.txt");
        public static string GetData()
        {
            string line = string.Empty;
            using (var sr = new StreamReader(file))
            {
                line = sr.ReadLine();
            }

            return line;
        }

        public static void WriteData(string data)
        {
           // using (FileStream fs = File.Open(file, FileMode.Open, FileAccess.Write, FileShare.ReadWrite))
            

                using (StreamWriter sw = File.AppendText(file))
            {
                sw.WriteLine(data);
            }
        }
       

        public static int GetRandomNumber()
        {
            Random r = new Random();
            return r.Next(10, 50);
        }

        public static Guid GetGuid()
        {
            return Guid.NewGuid();
        }

        //public static async Task<int> ThrowExceptionAsync()
        //{
        //    int i = 0;
        //    return 100 / i;
        //}
    }
}
