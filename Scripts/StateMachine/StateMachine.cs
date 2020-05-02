using System;
using Bale007.Enumerable;
using UnityEngine;

namespace Bale007.StateMachine
{
    public class StateMachine<T>
    {
        private CustomStack<T> statePool;
        private string targetName;

        public StateMachine(string targetName)
        {
            statePool = new CustomStack<T>();
            this.targetName = targetName;
        }

        public T CurrentState { get; protected set; }
        public event Action<T> OnStateChangeEvent;

        public virtual void PushState(T newState)
        {
            if (newState.Equals(CurrentState))
            {
                // Debug.Log("Your are tring to push the same state:" + newState + " to the "+ typeof(T).ToString());
            }

            statePool.Push(newState);

            CurrentState = statePool.Peek();

            //Debug.LogFormat("Object [{0}] State Changed to {1}", targetName, newState.ToString());
        }

        public virtual void PopState(T newState)
        {
            if (!statePool.Contains(newState))
            {
                Debug.Log("Missing State Found:" + newState);
            }
            else
            {
                statePool.Remove(newState);

                if (statePool.items.Count > 0) CurrentState = statePool.Peek();
            }
        }

        public virtual void Reset(T beginState)
        {
            statePool = new CustomStack<T>();

            PushState(beginState);
        }
    }
}