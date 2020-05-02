using System;
using UnityEngine;

namespace Bale007.Bindables
{
    public class BindableString
    {
        public event Action<string> changed;
        
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
    }
}