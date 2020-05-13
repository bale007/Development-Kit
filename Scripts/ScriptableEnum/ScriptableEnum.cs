using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bale007.ScriptableEnum
{
    [CreateAssetMenu(menuName = "Enum/New Value", fileName = "New Enum")]
    public class ScriptableEnum : ScriptableObject
    {
        public string Id
        {
            get { return name; }
        }
    }
}
