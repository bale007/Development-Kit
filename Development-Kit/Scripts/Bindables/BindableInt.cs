using System;

namespace Bale007.Bindables
{
    public class BindableInt
    {
        public event Action<int> changed;
        
        private int value;

        public BindableInt(int value = 0)
        {
            this.value = value;
        }

        public int Value
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
    }
}