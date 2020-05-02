using System;
using UnityEngine;

namespace Bale007.Bindables
{
    public class BindableVector3
    {
        public event Action<Vector3> changed;
        
        private Vector3 value;

        public BindableVector3(Vector3 value = default)
        {
            this.value = value;
        }

        public void RaiseEvent(Vector3 pos)
        {
            changed?.Invoke(pos);
        }

        public Vector3 Value
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
        
        public void Z(float z)
        {
            if (Math.Abs(this.value.z - z) > 0.0000001)
            {
                this.value.z = z;
                changed?.Invoke(value);
            }
        }
    }
}