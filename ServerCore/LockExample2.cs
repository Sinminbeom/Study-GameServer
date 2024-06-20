using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    internal class LockExample2
    {
        /*
        static volatile int number = 0;
        static volatile object _obj = new object();

        static void Thread_1()
        {
            for (int i = 0; i < 100000; i++)
            {
                // 상호배제 mutal Exclusive
                //Monitor.Enter(_obj);
                //number++;
                //Monitor.Exit(_obj);

                //try
                //{
                //    Monitor.Enter(_obj);
                //    number++;

                //    //return;
                //}
                //finally
                //{
                //    Monitor.Exit(_obj);
                //}

                lock (_obj)
                {
                    number++;
                }
            }

        }

        static void Thread_2()
        {
            for (int i = 0; i < 100000; i++)
            {
                //Monitor.Enter(_obj);
                //number--;
                //Monitor.Exit(_obj);

                lock (_obj)
                {
                    number--;
                }
            }
        }

        static void Main(string[] args)
        {
            System.Threading.Tasks.Task t1 = new System.Threading.Tasks.Task(Thread_1);
            System.Threading.Tasks.Task t2 = new System.Threading.Tasks.Task(Thread_2);
            t1.Start();
            t2.Start();

            System.Threading.Tasks.Task.WaitAll(t1, t2);

            Console.WriteLine(number);

        }
        */
    }
}
