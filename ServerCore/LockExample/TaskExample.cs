﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore.example
{
    internal class TaskExample
    {
        /*
        volatile static bool _stop = false;
        static void ThreadMain()
        {
            Console.WriteLine("쓰레드 시작!");

            while (_stop == false)
            {
                
            }

            Console.WriteLine("쓰레드 종료!");
        }
        static void Main(string[] args)
        {
            Task t = new Task(ThreadMain);
            t.Start();

            Thread.Sleep(1000);

            _stop = true;

            Console.WriteLine("Stop 호출");
            Console.WriteLine("종료 대기중");

            t.Wait();

            Console.WriteLine("종료 성공");
        }
        */
    }
}
