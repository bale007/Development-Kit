using System;

namespace Bale007.Bindables
{
    public class BindableString
    {
        private string value;

        public BindableString(string value = null)
        {
            this.value = value;
        }

        public string Value
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

        public event Action<string> changed;
    }
}