using System;
using System.Collections.Generic;

namespace Bale007.EventBus
{
    public static class EventBus
    {
        private static readonly Dictionary<EventBusEventType, List<Action<object[]>>> listenerGroups
            = new Dictionary<EventBusEventType, List<Action<object[]>>>();

        public static void Subscribe(EventBusEventType key, Action<object[]> listener)
        {
            if (!listenerGroups.TryGetValue(key, out var listenerList))
            {
                listenerList = new List<Action<object[]>>();
                listenerGroups.Add(key, listenerList);
            }

            listenerList.Add(listener);
        }

        public static void UnSubscribe(EventBusEventType key, Action<object[]> listener)
        {
            if (listenerGroups.TryGetValue(key, out var listenerList)) listenerList.Remove(listener);
        }

        public static void Publish(EventBusEventType key, params object[] parameters)
        {
            if (listenerGroups.TryGetValue(key, out var listenerList))
                foreach (var listener in listenerList.ToArray())
                    listener.Invoke(parameters);
        }

        public static void Clear()
        {
            listenerGroups.Clear();
        }
    }
}