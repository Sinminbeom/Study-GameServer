using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    internal class LockExample3
    {
        /*
        // 1) 무작정 기다린다(존버 메타)
        // 2) 일단 자리로, 나중에 다시(랜덤 메타)
        // 3) 난 소중하니 직원한테 부탁(갑질 메타)
        class FastLock
        {
            public int id;
        }
        class SessionManager
        {
            FastLock l;
            static object _lock = new object();

            public static void TestSession()
            {
                lock (_lock)
                {

                }
            }

            public static void Test()
            {
                lock (_lock)
                {
                    UserManager.TestUser();
                }
            }
        }

        class UserManager
        {
            FastLock l;
            static object _lock = new object();

            public static void Test()
            {
                lock (_lock)
                {
                    SessionManager.TestSession();
                }
            }

            public static void TestUser()
            {
                lock (_lock)
                {

                }
            }
        }
        static volatile int number = 0;
        static volatile object _obj = new object();

        static void Thread_1()
        {
            for (int i = 0; i < 100; i++)
            {
                SessionManager.Test();
            }

        }

        static void Thread_2()
        {
            for (int i = 0; i < 100; i++)
            {
                UserManager.Test();
            }
        }

        static void Main(string[] args)
        {
            Thread t1 = new Thread(Thread_1);
            Thread t2 = new Thread(Thread_2);

            t1.Name = "Test1";
            t2.Name = "Test2";

            t1.Start();

            Thread.Sleep(100);

            t2.Start();

            t1.Join();
            t2.Join();
            //Task.WaitAll(t1, t2);

            Console.WriteLine(number);

        }
        */
    }
}
