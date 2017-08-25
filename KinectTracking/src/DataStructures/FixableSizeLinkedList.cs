using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinectTracking.src.DataStructures
{
    public class FixableSizeLinkedList<T>
    {
        LinkedList<T> list = new LinkedList<T>();
        private int? limit = null;

        public FixableSizeLinkedList() { }
        public FixableSizeLinkedList(int limit)
        {
            this.limit = limit;
        }

        public int Count()
        {
            return list.Count();
        }

        public void AddLast(T item)
        {
            list.AddLast(item);
            if(hasLimit())
            {
                while (list.Count > limit)
                {
                    list.RemoveFirst();
                }
            }
        }

        private bool hasLimit()
        {
            if (limit != null) { return true; }
            else { return false; }            
        }

        public IEnumerator GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public Boolean isFull()
        {
            if (list.Count == limit) { return true; }
            else { return false; }
        }
    }
}
