﻿using System;

namespace ServerCore.example
{
    /*
    class Lock
    {
        // bool <- 커널
        AutoResetEvent _available = new AutoResetEvent(true);

        public void Acquire()
        {
            _available.WaitOne(); // 입장시도
        }

        public void Release()
        {
            _available.Set(); // flag = true

        }
    }
    internal class AutoResetEventExample
    {
        static int _num = 0;
        static Lock _lock = new Lock();

        static void Thread_1()
        {
            for (int i = 0; i < 1000000; i++)
            {
                _lock.Acquire();
                _num++;
                _lock.Release();
            }
        }

        static void Thread_2()
        {
            for (int i = 0; i < 1000000; i++)
            {
                _lock.Acquire();
                _num--;
                _lock.Release();
            }
        }

        static void Main(string[] args)
        {
            Task t1 = new Task(Thread_1);
            Task t2 = new Task(Thread_2);

            t1.Start();
            t2.Start();

            Task.WaitAll(t1, t2);

            Console.WriteLine(_num);
        }
    }
    */
}
