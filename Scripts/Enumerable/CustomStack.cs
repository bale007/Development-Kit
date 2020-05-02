using System.Collections.Generic;

namespace Bale007.Enumerable
{
    public class CustomStack<T>
    {
        public List<T> items = new List<T>();

        public void Push(T item)
        {
            items.Add(item);
        }

        public T Peek()
        {
            return items[items.Count - 1];
        }

        public T Pop()
        {
            if (items.Count > 0)
            {
                var temp = items[items.Count - 1];
                items.RemoveAt(items.Count - 1);
                return temp;
            }

            return default;
        }

        public void RemoveAt(int itemAtPosition)
        {
            items.RemoveAt(itemAtPosition);
        }

        public void Remove(T state)
        {
            items.Remove(state);
        }

        public bool Contains(T state)
        {
            return items.Contains(state);
        }
    }
}