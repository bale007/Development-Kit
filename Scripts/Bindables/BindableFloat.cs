using System;
using UnityEngine;

namespace Bale007.Bindables
{
    public class BindableFloat
    {
        public float maxValue;

        public float minValue;

        private float value;

        public BindableFloat(float value = 0f, float minValue = float.NegativeInfinity,
            float maxValue = float.PositiveInfinity)
        {
            this.value = value;

            this.minValue = minValue;
            this.maxValue = maxValue;
        }

        public float Value
        {
            get => value;
            set
            {
                value = Mathf.Max(minValue, Math.Min(value, maxValue));
                if (Math.Abs(this.value - value) > 0.0000001)
                {
                    this.value = value;
                    changed?.Invoke(value);
                }
            }
        }

        public event Action<float> changed;
    }
}