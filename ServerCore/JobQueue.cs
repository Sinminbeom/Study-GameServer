using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    public interface IJobQueue
    {
        void Push(Action job);
        Action Pop();
    }
    public class JobQueue: IJobQueue
    {
        Queue<Action> _jobQueue = new Queue<Action>();
        object _lock = new object();

        public Action Pop()
        {
            lock (_lock)
            {
                if (_jobQueue.Count == 0)
                    return null;

                return _jobQueue.Dequeue();
            }
        }

        public void Push(Action job)
        {
            lock (_lock)
                _jobQueue.Enqueue(job);
        }
    }
}
