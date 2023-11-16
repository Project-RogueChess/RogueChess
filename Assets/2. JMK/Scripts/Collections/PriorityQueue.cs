using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMK.Collections.Generic
{

    class PriorityQueue<T> where T : IComparable<T>
    {
        List<T> _heap = new List<T>();

        public int Count => _heap.Count;

        public void Push(T item)
        {
            _heap.Add(item);

            int current = _heap.Count - 1;

            while (current > 0)
            {
                int next = (current - 1) / 2;
                if (_heap[current].CompareTo(_heap[next]) > 0)
                    break;

                T temp = _heap[current];
                _heap[current] = _heap[next];
                _heap[next] = temp;

                current = next;
            }
        }

        public T Pop()
        {
            T item = _heap[0];

            int lastIndex = _heap.Count - 1;
            _heap[0] = _heap[lastIndex];
            _heap.RemoveAt(lastIndex);
            lastIndex--;

            int current = 0;
            while (true)
            {
                int left = 2 * current + 1;
                int right = 2 * current + 2;

                int next = current;
                if (left <= lastIndex && _heap[next].CompareTo(_heap[left]) > 0)
                    next = left;
                if (right <= lastIndex && _heap[next].CompareTo(_heap[right]) > 0)
                    next = right;

                if (next == current)
                    break;

                T temp = _heap[current];
                _heap[current] = _heap[next];
                _heap[next] = temp;

                current = next;
            }

            return item;
        }
    }

}