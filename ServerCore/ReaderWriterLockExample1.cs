using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    /*
    internal class ReaderWriterLockExample1
    {
        // 1. 근성
        // 2. 양보
        // 3. 갑질
        static object _lock1 = new object(); // Monitor
        static SpinLock _lock2 = new SpinLock(); // 1번과 2번 혼합된 객체(양보를 조금씩 하는게 성능상 더 이득)
        static Mutex _lock3 = new Mutex();
        static ReaderWriterLockSlim _lock4 = new ReaderWriterLockSlim();

        class Reward
        {

        }

        static Reward GetRewardById(int id)
        {
            _lock4.EnterReadLock();

            _lock4.ExitReadLock();
            return null;
        }

        static void AddReward(Reward reward)
        {
            _lock4.EnterWriteLock();

            _lock4.ExitWriteLock();
        }


        static void Main(string[] args)
        {
            lock (_lock1)
            {

            }

            bool lockTaken = false;
            try
            {
                _lock2.Enter(ref lockTaken);
            }
            finally
            {
                if (lockTaken)
                    _lock2.Exit();
            }
        }
    }
    */
}
