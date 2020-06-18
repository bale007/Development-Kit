using System;
using Bale007.Enumerable;
using UnityEngine;

namespace Bale007.StateMachine
{
    public class StateMachine<T> where T: Enum
    {
        public event Action<T> OnStateEnter;
        public event Action<T> OnStateExit;

        private T currentState;
        private T lastState;
        private readonly T defaultState;

        public T CurrentState
        {
            get { return currentState; }
        }

        public T LastState
        {
            get { return lastState; }
        }

        public StateMachine(T defaultState)
        {
            this.defaultState = defaultState;
        }

        public virtual void ChangeState(T newState, bool sendEvent = true)
        {
            if (Equals(currentState, newState))
            {
                return;
            }
            
            OnStateExit?.Invoke(currentState);

            lastState = currentState;
            currentState = newState;
            
            OnStateEnter?.Invoke(currentState);
        }

        public virtual void ResetState(bool sendEvent = true)
        {
            ChangeState(defaultState, sendEvent);
        }
    }
}