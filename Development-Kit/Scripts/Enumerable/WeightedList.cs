using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bale007.Enumerable
{  
    [System.Serializable]
    public class WeightedList<T> where T: class
    {
        [SerializeField]
        private List<WeightedItem<T>> data;
    
        int randomWeight;
        int weightCounter;
        int weightSum;
    
        public int Count { get { return data.Count; } }
    
        public WeightedList()
        {
            data = new List<WeightedItem<T>>();
    
            weightSum = 0;
        }
    
    
        public void Add(T item, int weight)
        {
            data.Add(new WeightedItem<T>(item, weight));
    
            weightSum += weight;
        }
    
        public void Remove(T toRemove)
        {
            foreach(WeightedItem<T> item in data)
            {
                if(toRemove == item.obj)
                {
                    weightSum -= item.weight;
    
                    data.Remove(item);
    
                    return;
                }
            }
        }
    
        public void Clear()
        {
            data.Clear();
    
            weightSum = 0;
        }
    
        public bool Contains(T toFound)
        {
            foreach (WeightedItem<T> item in data)
            {
                if(toFound == item.obj)
                {
                    return true;
                }
            }
    
            return false;
        }
        
        public T GetRandom()
        {
            randomWeight = Random.Range(0, weightSum);
            weightCounter = 0;
    
            foreach (WeightedItem<T> item in data)
            {
                weightCounter += item.weight;
    
                if (randomWeight < weightCounter)
                {
                    return item.obj;
                }
            }
    
            Debug.LogErrorFormat("Object Not Found, RandomWeight {0}, WeightSum {1}, WeightCounter {2}",
                randomWeight, weightSum, weightCounter);
    
            return null as T;
        }   
    
        public List<T> GetOriginList()
        {
            List<T> list = new List<T>();
    
            foreach(WeightedItem<T> item in data)
            {
                list.Add(item.obj);
            }
    
            return list;
        }
    
        [System.Serializable]
        public class WeightedItem<T>
        {
            public T obj;
            public int weight;
    
            public WeightedItem(T obj, int weight)
            {
                this.obj = obj;
                this.weight = weight;
            }
        }
    }
}
