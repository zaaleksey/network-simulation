using System.Collections.Generic;
using System.Linq;

namespace NetworkSimulator
{
    public class QueueBuffer : Buffer
    {
        private Queue<Fragment> fragments;

        //Создает очередь
        public QueueBuffer()
        {
            fragments = new Queue<Fragment>();
        }

        public override bool IsEmpty()
        {
            if (fragments.Count() == 0)
            {
                return true;
            }
            return false;
        }

        public override int Count()
        {
            return fragments.Count();
        }

        public override void Put(Fragment fragment)
        {
            fragments.Enqueue(fragment);
        }

        public override Fragment Take()
        {
            return fragments.Dequeue();
        }
    }
}
