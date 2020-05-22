using System;
using System.Collections.Generic;
using Bale007.Util;
using UnityEngine;

namespace Bale007.Param
{
    public class Param
    {
        private readonly Dictionary<string, string> paramData;
        
        public Param(string data, char rowChar = '#', char pairChar = '@')
        {
            if (string.IsNullOrEmpty(data))
            {
                paramData = new Dictionary<string, string>();
                
                ClientUtil.Log("Param Data Empty","yellow");
            }
            else
            {
                paramData = ClientUtil.StringToStringDictionary(data, rowChar, pairChar);
            }
        }

        public string GetString(string paramKey, string defaultValue)
        {
            if (paramData.TryGetValue(paramKey, out var vo))
            {
                return vo;
            }
            
            return defaultValue;
        }

        public Vector2 GetVector(string paramKey, Vector2 defaultValue)
        {
            if (paramData.TryGetValue(paramKey, out var vo))
            {
                return ClientUtil.ParseVector2(vo);
            }
            
            return defaultValue;
        }

        public float GetFloat(string paramKey, float defaultValue)
        {
            if (paramData.TryGetValue(paramKey, out var vo))
            {
                return ClientUtil.ParseFloat(vo);
            }
            
            return defaultValue;
        }
        
        public int GetInt(string paramKey, int defaultValue)
        {
            if (paramData.TryGetValue(paramKey, out var vo))
            {
                return ClientUtil.ParseInt(vo);
            }
            
            return defaultValue;
        }
        
        public bool GetBool(string paramKey, bool defaultValue)
        {
            if (paramData.TryGetValue(paramKey, out var vo))
            {
                return ClientUtil.ParseBool(vo);
            }
            
            return defaultValue;
        }
        
        public T GetEnum<T>(string paramKey, T defaultValue) where T: Enum
        {
            if (paramData.TryGetValue(paramKey, out var vo))
            {
                return ClientUtil.ParseEnum<T>(vo);
            }
            
            return defaultValue;
        }
    }
}
