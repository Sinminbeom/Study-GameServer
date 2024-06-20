using System;

namespace Server
{
    internal class ThreadExample
    {
        /*
        static void MainThread(object state)
        {
            for (int i = 0; i < 5; i++)
                Console.WriteLine("Hello Thread!");
        }
        static void Main(string[] args)
        {
            ThreadPool.SetMinThreads(1, 1);
            ThreadPool.SetMaxThreads(5, 5);

            for (int i = 0; i < 5; i++)
            {
                //Task task = new Task(() => { while (true) { } }, TaskCreationOptions.LongRunning);
                Task task = new Task(() => { while (true) { } });
                task.Start();
            }

            //for (int i = 0; i < 4; i++)
            //    ThreadPool.QueueUserWorkItem((obj) => { while (true) { } });

            ThreadPool.QueueUserWorkItem(MainThread);

            //for (int i = 0; i < 1000; i++)
            //{
            //    Thread thread = new Thread(MainThread);
            //    thread.IsBackground = true;
            //    thread.Start();
            //}
            //thread.Name = "Test Thread";

            //thread.Join();
            //Console.WriteLine("Hello World!");

            while (true)
            {

            }
        }
        */
    }
}
