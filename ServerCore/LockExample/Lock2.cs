﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore.example
{
    // 재귀적 락을 허용할지 (Yes) WriteLock -> WriteLock OK, WriteLock->ReadLock OK, ReadLock->WriteLock NO
    // 스핀락 정책 (5000번 -> Yield)
    internal class Lock2
    {
        const int EMPTY_FLAG = 0x00000000;
        const int WRITE_MASK = 0x7FFF0000;
        const int READ_MASK = 0x0000FFFF;
        const int MAX_SPIN_COUNT = 5000;
        // 32비트
        // [Unused(1비트)] [WriteThreadId(15비트)] [ReadCount(16비트)]
        int _flag = EMPTY_FLAG;
        int _writeCount = 0;

        public void WriteLock()
        {
            // 동일 쓰레드가 WriteLock을 이미 획득하고 있는지 확인
            int lockThreadId = (_flag & WRITE_MASK) >> 16;
            int currentLockThreadId = Thread.CurrentThread.ManagedThreadId;
            if (currentLockThreadId == lockThreadId)
            {
                _writeCount++;
                return;
            }


            // 아무도 WriteLock or ReadLock을 획득하고 있지 않을 때, 경합해서 소유권을 얻는다.
            int desired = Thread.CurrentThread.ManagedThreadId << 16 & WRITE_MASK;
            while (true)
            {
                for (int i = 0; i < MAX_SPIN_COUNT; i++)
                {
                    // 시도를 해서 성공하면 return

                    //if (_flag == EMPTY_FLAG)
                    //    _flag = desired;

                    if (Interlocked.CompareExchange(ref _flag, desired, EMPTY_FLAG) == EMPTY_FLAG)
                    {
                        _writeCount = 1;
                        return;
                    }
                }

                Thread.Yield();
            }

        }

        public void WriteUnLock()
        {
            int lockCount = --_writeCount;
            if (lockCount == 0)
                Interlocked.Exchange(ref _flag, EMPTY_FLAG);
        }

        public void ReadLock()
        {
            // 동일 쓰레드가 WriteLock을 이미 획득하고 있는지 확인
            int lockThreadId = (_flag & WRITE_MASK) >> 16;
            int currentLockThreadId = Thread.CurrentThread.ManagedThreadId;
            if (currentLockThreadId == lockThreadId)
            {
                Interlocked.Increment(ref _flag);
                return;
            }

            while (true)
            {
                for (int i = 0; i < MAX_SPIN_COUNT; i++)
                {
                    // 아무도 Wirte을 획득하고 있지 않으면 ReadCount를 1 늘린다.
                    int expected = _flag & READ_MASK;
                    if (Interlocked.CompareExchange(ref _flag, expected + 1, expected) == expected)
                        return;

                }
                Thread.Yield();
            }
        }

        public void ReadUnLock()
        {
            Interlocked.Decrement(ref _flag);
        }
    }



}
