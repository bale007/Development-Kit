using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Bale007.Bindables
{
    public class BindableEnum<T> where T: Enum
    {
        public event Action<T> changed;
        
        private T value;

        public BindableEnum(T value)
        {
            this.value = value;
        }

        public T Value
        {
            get => value;
            set
            {
                if (!this.value.Equals(value))
                {
                    this.value = value;
                    changed?.Invoke(value);
                }
            }
        }
    }
}