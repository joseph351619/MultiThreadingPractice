using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MultiThreadingPractice
{
    class Program
    {
        bool done;
        static readonly object locker = new object();

        static void Main(string[] args)
        {
            ////Thread t = new Thread(WriteY);
            ////t.Start();
            //var pro = new Program();
            //new Thread(pro.WriteY).Start();
            //pro.WriteY();
            ////if (!done)
            ////    for (int i = 0; i < 1000; i++) Console.Write("x");

            //Thread worker = new Thread(() => Console.ReadLine());
            //if (args.Length == 0) worker.IsBackground = true;
            //worker.Start();

            //        Task<string> task = Task.Factory.StartNew<string>
            //(() => DownloadString("http://www.linqpad.net"));

            //        // We can do other work here and it will execute in parallel:
            //        WriteY();

            // When we need the task's return value, we query its Result property:
            // If it's still executing, the current thread will now block (wait)
            // until the task finishes:
            //string result = task.Result;
            //Console.WriteLine(result);
            //Console.ReadKey();

            ReadAsync.Run();
        }
        static string DownloadString(string uri)
        {
            using (var wc = new System.Net.WebClient())
                return wc.DownloadString(uri);
        }
        static void WriteY()
        {
            //lock (locker)
            //if (!done)
                    for (int i = 0; i < 1000; i++)
                    {
                        Console.Write("y");
                    }
            //{
            //    Console.WriteLine("done");
            //    done = true;
            //}
        }
    }
}
