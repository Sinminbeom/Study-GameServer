using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore.example
{
    /*
    class SpinLock // 유저 동기화 객체
    {
        volatile int _locked = 0;

        public void Acquire()
        {
            while (true)
            {
                // CAS Compare-And-Swap
                //if (_locked == 0)
                //{
                //    _locked = 1;
                //}
                int expected = 0;
                int desired = 1;

                //int original = Interlocked.CompareExchange(ref _locked, desired, expected);
                //if (original == 0)
                //    break;

                if (Interlocked.CompareExchange(ref _locked, desired, expected) == expected)
                    break;


                Thread.Sleep(1); // 무조건 휴식 => 무조건 1ms 정도 쉬고 싶어요
                Thread.Sleep(0); // 조건부 양보 => 나보다 우선순위가 낮은 애들한테는 양보 불가 => 우선순위가 나보다 같거나 높은 쓰레드가 없으면 다시 본인한테
                Thread.Yield(); // 관대한 양보 => 관대하게 양보할테니, 지금 실행이 가능한 쓰레드가 있으면 실행하세요 => 실행 가능한 애가 없으면 남은 시간 소진
            }


            // 멀티 스레드환경에서 두개로 나눠져서 그런거
            // 두개의 로직을 하나로 합치는게 관건!!!
            // 이걸 해결해주는게 Interlocked.Exchange

            //while (true)
            //{
            //    int original = Interlocked.Exchange(ref _locked, 1);
            //    if (original == 0)
            //        break;
            //}

            //while (_locked)
            //{
            //    // 잠김이 풀리기를 기다린다.
            //}

            //// 내꺼
            //_locked = true;
        }

        public void Release()
        {
            _locked = 0;

        }
    }
    internal class SpinLockExample
    {
        static int _num = 0;
        static SpinLock _lock = new SpinLock();

        static void Thread_1()
        {
            for (int i = 0; i < 100000; i++)
            {
                _lock.Acquire();
                _num++;
                _lock.Release();
            }
        }

        static void Thread_2()
        {
            for (int i = 0; i < 100000; i++)
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
