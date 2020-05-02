using System;
using UnityEngine;

namespace Bale007.Bindables
{
    public class BindableVector2
    {
        public event Action<Vector2> changed;
        
        private Vector2 value;

        public BindableVector2(Vector2 value = default)
        {
            this.value = value;
        }

        public Vector2 Value
        {
            get => value;
            set
            {
                if (this.value != value)
                {
                    this.value = value;
                    changed?.Invoke(value);
                }
            }
        }

        public void X(float x)
        {
            if (Math.Abs(this.value.x - x) > 0.0000001)
            {
                this.value.x = x;
                changed?.Invoke(value);
            }
        }
        
        public void Y(float y)
        {
            if (Math.Abs(this.value.y - y) > 0.0000001)
            {
                this.value.y = y;
                changed?.Invoke(value);
            }
        }
    }
}