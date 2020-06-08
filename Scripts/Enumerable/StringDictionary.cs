using System.Collections.Generic;
using Data;
using Sirenix.OdinInspector;

namespace Enumerable
{
    public class StringDictionary<T>
    {
        private Dictionary<string, T> data;
    
        public StringDictionary()
        {
            data = new Dictionary<string, T>();
        }

        public void Clear()
        {
            data.Clear();
        }

        public void TrySetValue(string key, T value)
        {
            if (!data.ContainsKey(key))
            {
                data.Add(key, value);
            }
            else
            {
                data[key] = value;
            }
        }

        public T TryGetValue(string key, T def)
        {
            if(data.TryGetValue(key, out var result))
            {
                return (T) result;
            }

            return def;
        }
    }
}
