using System;
using UnityEngine;

namespace Bale007.Bindables
{
    public class BindableVector3
    {
        private Vector3 value;

        public BindableVector3(Vector3 value = default)
        {
            this.value = value;
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

        public event Action<Vector3> changed;

        public void RaiseEvent(Vector3 pos)
        {
            changed?.Invoke(pos);
        }

        public void X(float x)
        {
            if (Math.Abs(value.x - x) > 0.0000001)
            {
                value.x = x;
                changed?.Invoke(value);
            }
        }

        public void Y(float y)
        {
            if (Math.Abs(value.y - y) > 0.0000001)
            {
                value.y = y;
                changed?.Invoke(value);
            }
        }

        public void Z(float z)
        {
            if (Math.Abs(value.z - z) > 0.0000001)
            {
                value.z = z;
                changed?.Invoke(value);
            }
        }
    }
}