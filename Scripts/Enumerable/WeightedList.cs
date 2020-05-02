using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Bale007.Enumerable
{
    [Serializable]
    public class WeightedList<T> where T : class
    {
        [SerializeField] private List<WeightedItem<T>> data;

        private int randomWeight;
        private int weightCounter;
        private int weightSum;

        public WeightedList()
        {
            data = new List<WeightedItem<T>>();

            weightSum = 0;
        }

        public int Count => data.Count;


        public void Add(T item, int weight)
        {
            data.Add(new WeightedItem<T>(item, weight));

            weightSum += weight;
        }

        public void Remove(T toRemove)
        {
            foreach (var item in data)
                if (toRemove == item.obj)
                {
                    weightSum -= item.weight;

                    data.Remove(item);

                    return;
                }
        }

        public void Clear()
        {
            data.Clear();

            weightSum = 0;
        }

        public bool Contains(T toFound)
        {
            foreach (var item in data)
                if (toFound == item.obj)
                    return true;

            return false;
        }

        public T GetRandom()
        {
            randomWeight = Random.Range(0, weightSum);
            weightCounter = 0;

            foreach (var item in data)
            {
                weightCounter += item.weight;

                if (randomWeight < weightCounter) return item.obj;
            }

            Debug.LogErrorFormat("Object Not Found, RandomWeight {0}, WeightSum {1}, WeightCounter {2}",
                randomWeight, weightSum, weightCounter);

            return null;
        }

        public List<T> GetOriginList()
        {
            var list = new List<T>();

            foreach (var item in data) list.Add(item.obj);

            return list;
        }

        [Serializable]
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