using System;

namespace Bale007.Bindables
{
    public class BindableObject
    {
        private object value;

        public BindableObject(object value = null)
        {
            this.value = value;
        }

        public object Value
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

        public event Action<object> changed;
    }
}