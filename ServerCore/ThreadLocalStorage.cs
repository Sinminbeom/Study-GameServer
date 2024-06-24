using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    internal class ThreadLocalStorage
    {
        static ThreadLocal<string> threadName = new ThreadLocal<string>(() => { return $"My Name Is {Thread.CurrentThread.ManagedThreadId}"; });
        //static string threadName;

        static void WhoAmI()
        {
            bool repeat = threadName.IsValueCreated;
            if (repeat)
                Console.WriteLine(threadName.Value + " (repeat)");
            else
                Console.WriteLine(threadName.Value);
            //threadName.Value = $"My Name Is {Thread.CurrentThread.ManagedThreadId}";
            //threadName = $"My Name Is {Thread.CurrentThread.ManagedThreadId}";

            Thread.Sleep(1000);

            Console.WriteLine(threadName.Value);
            //Console.WriteLine(threadName);
        }

        static void Main(string[] args)
        {
            ThreadPool.SetMinThreads(1, 1);
            ThreadPool.SetMaxThreads(3, 3);
            Parallel.Invoke(WhoAmI, WhoAmI, WhoAmI, WhoAmI, WhoAmI, WhoAmI, WhoAmI);

            threadName.Dispose();
        }
    }
}
