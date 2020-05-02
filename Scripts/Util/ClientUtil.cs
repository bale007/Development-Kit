using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Bale007.Util
{
    public static class ClientUtil
    {              
        #region Debug

        public static void ShowSystemInfo()
        {
            Debug.Log("## System memory: " + SystemInfo.systemMemorySize.ToString() + " MB");
            Debug.Log("## Graphics API: " + SystemInfo.graphicsDeviceVersion);
        }
    
        public static void Log(string aLog)
        {
            if (Setting.SHOW_DEBUG_MSG)
            {
                Debug.Log(aLog);
            }
        }

        public static void LogError(string aLog)
        {
            if (Setting.SHOW_DEBUG_MSG)
            {
                Debug.LogError(aLog);
            }
        }

        public static void Log(string aLog, string color)
        {
            if (Setting.SHOW_DEBUG_MSG)
            {
                Debug.Log("<color=" + color + ">" + aLog + "</color>");
            }
        }

        #endregion debug

        #region Parsing

        public static T ParseEnum<T>(string value) where T : System.Enum
        {
            try
            {
                return (T)System.Enum.Parse(typeof(T), value);
            }
            catch
            {
                throw new System.Exception("Error Happended When Parsing:" + value);
            }
        }

        public static int ParseInt(string aText)
        {
            return int.Parse(aText, System.Globalization.NumberFormatInfo.InvariantInfo);
        }

        public static float ParseFloat(string aText)
        {
            return float.Parse(aText, System.Globalization.NumberFormatInfo.InvariantInfo);
        }

        public static Vector2 ParseVector2(string aText)
        {
            string[] textSplit = aText.Split(',');
            return new Vector2(ParseFloat(textSplit[0]), ParseFloat(textSplit[1]));
        }

        public static string ToUpper(string aText)
        {
            return aText.ToUpper(System.Globalization.CultureInfo.InvariantCulture);
        }

        public static string ToLower(string aText)
        {
            return aText.ToLower(System.Globalization.CultureInfo.InvariantCulture);
        }

        public static int CompareTo(string a, string b)
        {
            return string.Compare(a, b, StringComparison.Ordinal);
        }

        #endregion parsing

        #region Conversion

        public static double ConvertBytesToMegabytes(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }

        public static long ConvertMegabytesToBytes(long megabytes)
        {
            return (megabytes * 1024) * 1024;
        }

        public static string ListToString(List<string> content, char separator = ',')
        {
            string result = "";
            
            if (content == null)
            {
                return null;
            }

            for (int i = 0; i < content.Count; i++)
            {
                if (i != 0)
                {
                    result += ",";
                }
                
                result += content[i];
            }

            return result;
        }
        
        public static string ListToString(List<int> content, char separator = ',')
        {
            string result = "";
            
            if (content == null)
            {
                return null;
            }

            for (int i = 0; i < content.Count; i++)
            {
                if (i != 0)
                {
                    result += ",";
                }
                
                result += content[i];
            }

            return result;
        }
        
        public static List<string> StringToList(string content, char separator = ',')
        {
            if (string.IsNullOrEmpty(content))
            {
                return null;
            }

            if (!content.Contains(separator))
            {
                return new List<string>{content};
            }

            return new List<string>(content.Split(separator));
        }
        
        public static List<int> StringToIntegerList(string content, char separator = ',')
        {
            if (string.IsNullOrEmpty(content))
            {
                return null;
            }

            if (!content.Contains(separator))
            {
                return new List<int>{int.Parse(content)};
            }
            
            var list = new List<int>();

            foreach (var value in content.Split(separator))
            {
                list.Add(int.Parse(value));
            }

            return list;
        }
        
        public static List<T> StringToList<T>(string content, char separator = ',') where T: Enum
        {
            if (string.IsNullOrEmpty(content))
            {
                return null;
            }
            
            if (!content.Contains(separator))
            {
                return new List<T>{ParseEnum<T>(content)};
            }

            var list = new List<T>();
            
            foreach (var str in StringToList(content))
            {
                list.Add(ParseEnum<T>(str));
            }

            return list;
        }

        public static string ColorToHex(Color32 color)
        {
            string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
            return hex;
        }

        public static Color HexToColor(string hex)
        {
            hex = hex.Replace("0x", "");//in case the string is formatted 0xFFFFFF
            hex = hex.Replace("#", "");//in case the string is formatted #FFFFFF
            byte a = 255;//assume fully visible unless specified in hex
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            //Only use alpha if the string has enough characters
            if (hex.Length == 8)
            {
                a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
            }
            return new Color32(r, g, b, a);
        }

        public static List<string> DictionaryToList(Dictionary<string, int> dictionary, char separator = ':')
        {
            List<string> result = new List<string>();

            foreach (var pair in dictionary)
            {
                result.Add(pair.Key.ToString() + separator + pair.Value.ToString());
            }

            return result;
        }
        
        public static List<string> DictionaryToList(Dictionary<int, int> dictionary, char separator = ':')
        {
            List<string> result = new List<string>();

            foreach (var pair in dictionary)
            {
                result.Add(pair.Key.ToString() + separator + pair.Value.ToString());
            }

            return result;
        }

        public static List<string> DictionaryToList<T>(Dictionary<T, int> dictionary, char separator = ':') where T : Enum
        {
            List<string> result = new List<string>();

            foreach (var pair in dictionary)
            {
                result.Add(pair.Key.ToString() + separator + pair.Value.ToString());
            }

            return result;
        }

        public static Dictionary<string, int> StringToIntDictionary(string content, char rowChar = '&', char pairChar = '=')
        {
            if (string.IsNullOrEmpty(content))
            {
                return null;
            }
        
            Dictionary<string, int> returnDic = new Dictionary<string, int>();

            List<string> rowString = StringToList(content, rowChar);

            foreach (string rowData in rowString)
            {
                var arr = rowData.Split(pairChar);

                returnDic.Add(arr[0], int.Parse(arr[1]));
            }

            return returnDic;
        }
    
        public static Dictionary<T, int> StringToIntDictionary<T>(string content, char rowChar = '&', char pairChar = '=') where T : Enum
        {
            if (string.IsNullOrEmpty(content))
            {
                return null;
            }
        
            Dictionary<T, int> returnDic = new Dictionary<T, int>();

            List<string> rowString = StringToList(content, rowChar);

            foreach (string rowData in rowString)
            {
                var arr = rowData.Split(pairChar);

                returnDic.Add(ParseEnum<T>(arr[0]), int.Parse(arr[1]));
            }

            return returnDic;
        }

        public static Dictionary<string, string> StringToStringDictionary(string content, char rowChar = '&', char pairChar = '=')
        {
            if (string.IsNullOrEmpty(content))
            {
                return null;
            }
        
            var returnDic = new Dictionary<string, string>();

            List<string> rowString = StringToList(content, rowChar);

            foreach (string rowData in rowString)
            {
                var arr = rowData.Split(pairChar);

                returnDic.Add(arr[0], arr[1]);
            }

            return returnDic;
        }
        
        public static Dictionary<int, string> StringToIntStringDictionary(string content, char rowChar = '&', char pairChar = '=')
        {
            if (string.IsNullOrEmpty(content))
            {
                return null;
            }
        
            var returnDic = new Dictionary<int, string>();

            List<string> rowString = StringToList(content, rowChar);

            foreach (string rowData in rowString)
            {
                var arr = rowData.Split(pairChar);

                returnDic.Add(int.Parse(arr[0]), arr[1]);
            }

            return returnDic;
        }
    
        public static Dictionary<T, string> StringToStringDictionary<T>(string content, char rowChar = '&', char pairChar = '=') where T : Enum
        {
            if (string.IsNullOrEmpty(content))
            {
                return null;
            }
        
            Dictionary<T, string> returnDic = new Dictionary<T, string>();

            List<string> rowString = StringToList(content, rowChar);

            foreach (string rowData in rowString)
            {
                var arr = rowData.Split(pairChar);

                returnDic.Add(ParseEnum<T>(arr[0]), arr[1]);
            }

            return returnDic;
        }
        
        public static Dictionary<string, T> StringToEnumDictionary<T>(string content, char rowChar = '&', char pairChar = '=') where T : Enum
        {
            if (string.IsNullOrEmpty(content))
            {
                return null;
            }
        
            var returnDic = new Dictionary<string, T>();

            List<string> rowString = StringToList(content, rowChar);

            foreach (string rowData in rowString)
            {
                var arr = rowData.Split(pairChar);

                returnDic.Add(arr[0], ParseEnum<T>(arr[1]));
            }

            return returnDic;
        }

        public static Dictionary<string, string> ListToStringDictionary(List<string> list, char separator = ':')
        {
            Dictionary<string, string> aDict = new Dictionary<string, string>();

            foreach (string content in list)
            {
                string[] values = content.Split(separator);

                aDict.Add(values[0], values[1]);
            }

            return aDict;
        }
        
        public static Dictionary<T, string> ListToStringDictionary<T>(List<string> list, char separator = ':') where T: Enum
        {
            Dictionary<T, string> aDict = new Dictionary<T, string>();

            foreach (string content in list)
            {
                string[] values = content.Split(separator);

                aDict.Add(ParseEnum<T>(values[0]), values[1]);
            }

            return aDict;
        }
        
        public static Dictionary<string, int> ListToStringIntDictionary(List<string> list, char separator = ':')
        {
            Dictionary<string, int> aDict = new Dictionary<string, int>();

            foreach (string content in list)
            {
                string[] values = content.Split(separator);

                aDict.Add(values[0], int.Parse(values[1]));
            }

            return aDict;
        }
        
        public static Dictionary<int, int> ListToIntIntDictionary(List<string> list, char separator = ':')
        {
            Dictionary<int, int> aDict = new Dictionary<int, int>();

            foreach (string content in list)
            {
                string[] values = content.Split(separator);

                aDict.Add(int.Parse(values[0]), int.Parse(values[1]));
            }

            return aDict;
        }

        #endregion conversion

        #region Display

        public static bool LayerInLayerMask(int layer, LayerMask layerMask)
        {
            if (((1 << layer) & layerMask) != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static int LayerMaskToLayer(LayerMask layerMask)
        {
            int layerNumber = 0;
            int layer = layerMask.value;
            while (layer > 0)
            {
                layer = layer >> 1;
                layerNumber++;
            }
            return layerNumber - 1;
        }

        public static string ReplaceTextEncoding(string aText)
        {
            if (aText == null || aText == "") return "";

            aText = aText.Replace("[colorRed]", "<color=#FF0000>");
            aText = aText.Replace("[colorEnd]", "</color>");

            aText = aText.Replace("[bold]", "<b>");
            aText = aText.Replace("[boldEnd]", "</b>");
            aText = aText.Replace("[italic]", "<i>");
            aText = aText.Replace("[italicEnd]", "</i>");

            return aText;
        }

        public static string FilterName(string aName)
        {
            string outputString = aName.Replace("[", "");
            outputString = outputString.Replace("]", "");
            outputString = outputString.Replace("<", "");
            outputString = outputString.Replace(">", "");
            outputString = outputString.Replace("\n", "");
            return outputString.Trim();
        }

        #endregion display

        #region format

        public static string CombinePath(string path, string filename)
        {
            return path + "/" + filename;
        }

        public static string FormatString(string str, Dictionary<string, string> parameters)
        {
            foreach (KeyValuePair<string, string> entry in parameters)
            {
                str = str.Replace(string.Format("{{{0}}}", entry.Key), entry.Value);
            }
            return str;
        }

        public static float Round(float value, int digits)
        {
            float mult = Mathf.Pow(10.0f, (float)digits);
            return Mathf.Round(value * mult) / mult;
        }

        #endregion format

        #region Math

        public static float ClampFloat(float num, float min, float max)
        {
            num = Mathf.Min(max, num);
            num = Mathf.Max(min, num);

            return num;
        }

        public static List<int> GetDmgRangeFromVariance(int baseDmg, float variance)
        {
            List<int> range = new List<int>();
            if (variance > 0)
            {
                int min = Mathf.RoundToInt((float)baseDmg * (1f - variance));
                int max = Mathf.RoundToInt((float)baseDmg * (1f + variance));

                range.Add(Mathf.Max(1, min));
                range.Add(Mathf.Max(1, max));
            }
            else
            {
                range.Add(Mathf.Max(1, baseDmg));
                range.Add(Mathf.Max(1, baseDmg));
            }

            return range;
        }

        public static int GetLowestIndex(List<float> valueList)
        {
            float lowest = float.MaxValue;
            int lowestIndex = -1;
            int checkIndex = 0;
            foreach (float value in valueList)
            {
                if (value < lowest)
                {
                    lowest = value;
                    lowestIndex = checkIndex;
                }
                checkIndex++;
            }

            return lowestIndex;
        }

        public static int GetIntListTotal(List<int> aList)
        {
            return aList.Sum();
        }

        public static int CalculateDimensionByRatio(int origDim, int scaledRefDim, int origRefDim)
        {
            int resultDim = Mathf.RoundToInt(scaledRefDim / (float)origRefDim * (float)origDim);

            return resultDim;
        }

        #endregion math

        #region Vectors

        public static Vector2 SnapPosition(Vector2 pos)
        {
            var rounded = new Vector2();

            rounded.x = Mathf.Round(pos.x);
            rounded.y = Mathf.Round(pos.y);

            rounded.x += pos.x >= rounded.x ? 0.5f : -0.5f;
            rounded.y += pos.y >= rounded.y ? 0.5f : -0.5f;

            return rounded;
        }

        public static Vector2 Rotate(this Vector2 v, float degrees)
        {
            float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
            float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

            float tx = v.x;
            float ty = v.y;
            v.x = (cos * tx) - (sin * ty);
            v.y = (sin * tx) + (cos * ty);
            return v;
        }

        public static Vector3 SnapVector(Vector3 v3, float snapAngle)
        {
            float angle = Vector3.Angle(v3, Vector3.up);
            if (angle < snapAngle / 2.0f)
                return Vector3.up * v3.magnitude;
            if (angle > 180.0f - snapAngle / 2.0f)
                return Vector3.down * v3.magnitude;

            float t = Mathf.Round(angle / snapAngle);
            float deltaAngle = (t * snapAngle) - angle;

            Vector3 axis = Vector3.Cross(Vector3.up, v3);
            Quaternion q = Quaternion.AngleAxis(deltaAngle, axis);
            return q * v3;
        }

        #endregion vectors

        #region Time

        public static string GetSystemTime()
        {
            DateTime dt = DateTime.Now;
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static DateTime GetSystemTimeV2()
        {
            return DateTime.Now;
        }

        public static string FormatDateTime(DateTime datetime)
        {
            return datetime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static TimeSpan GetTimeSpanFromNow(string to)
        {
            DateTime dt2 = DateTime.Parse(to);

            return GetTimeSpanFromNow(dt2);
        }

        public static TimeSpan GetTimeSpanFromNow(DateTime to)
        {
            DateTime dt1 = DateTime.Now;

            DateTime dt2 = to;

            TimeSpan ts1 = new TimeSpan(dt1.Ticks);

            TimeSpan ts2 = new TimeSpan(dt2.Ticks);

            TimeSpan tsSub = ts1.Subtract(ts2).Negate();

            return tsSub;
        }

        public static string GetTimeFormat(int aTime)
        {
            int hr = aTime / 3600;

            aTime -= (hr * 3600);

            int min = aTime / 60;

            aTime -= (min * 60);

            string hrString = (hr < 10) ? ("0" + hr) : hr.ToString();
            string minString = (min < 10) ? ("0" + min) : min.ToString();
            string secString = (aTime < 10) ? ("0" + aTime) : aTime.ToString();

            return hrString + ":" + minString + ":" + secString;
        }

        public static string GetHrFormat(int aHr)
        {
            if (aHr < 10)
            {
                return "0" + aHr + ":00";
            }
            else
            {
                return aHr + ":00";
            }
        }

        public static string GetMissionTimeFormat(float aMTime)
        {
            string tString = "";
            if (aMTime < 10f)
            {
                tString += "0" + Mathf.FloorToInt(aMTime).ToString();
            }
            else
            {
                tString += Mathf.FloorToInt(aMTime).ToString();
            }

            int minInt = Mathf.FloorToInt(aMTime % 1f * 60f);
            if (minInt < 10f)
            {
                tString += "0" + minInt.ToString();
            }
            else
            {
                tString += minInt.ToString();
            }

            return tString;
        }

        #endregion Time

        #region Random

        public static T RandomEnumValue<T>() where T : Enum
        {
            var values = Enum.GetValues(typeof(T));
            int random = UnityEngine.Random.Range(0, values.Length);
            return (T)values.GetValue(random);
        }

        public static T RandomFromList<T>(List<T> aList)
        {
            return aList[UnityEngine.Random.Range(0, aList.Count)];
        }

        public static int RandomNegativePositive()
        {
            return UnityEngine.Random.Range(0, 2) * 2 - 1;
        }

        public static Vector2 RandomNormalizedVector()
        {
            Vector2 v = Vector2.one;
            v.x = UnityEngine.Random.Range(-1f, 1f);
            v.y = UnityEngine.Random.Range(-1f, 1f);
            return v.normalized;
        }

        public static bool ChanceSuccess100(float chance)
        {
            return chance > UnityEngine.Random.Range(0, 100);
        }

        public static bool RandomizeChance(float chance)
        {
            if (chance <= 0)
            {
                return false;
            }
            else if (chance >= 1f)
            {
                return true;
            }
            else
            {
                float randomFloat = UnityEngine.Random.Range(0, 1f);
                if (randomFloat > chance)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public static List<int> ShuffleIntList(List<int> toShuffle, int selectCount)
        {
            List<int> shuffleIndexList = GetRandomIntList(toShuffle.Count, selectCount);
            List<int> shuffledList = new List<int>();
            foreach (int shuffleIndex in shuffleIndexList)
            {
                shuffledList.Add(toShuffle[shuffleIndex]);
            }

            return shuffledList;
        }

        public static List<int> SelectRandomIntList(List<int> intList, int selectCount)
        {
            List<int> selectList = new List<int>();
            List<int> indexList = GetRandomIntList(intList.Count, selectCount);

            foreach (int randInd in indexList)
            {
                selectList.Add(intList[randInd]);
            }

            return selectList;
        }

        public static int ChooseRandomIndex(int listCount)
        {
            return UnityEngine.Random.Range(0, listCount);
        }

        public static List<int> GetRandomIntList(int listCount, int selectCount)
        {
            List<int> selectList = new List<int>();
            List<int> randomList = new List<int>();

            selectCount = Math.Min(listCount, selectCount);

            for (int i = 0; i < listCount; i++)
            {
                randomList.Add(i);
            }

            while (randomList.Count > 0 && selectCount > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, randomList.Count);
                selectList.Add(randomList[randomIndex]);
                //			debug (randomIndex + " selectList add "+randomList[randomIndex]);

                randomList.RemoveAt(randomIndex);
                selectCount -= 1;
            }

            return selectList;
        }      

        #endregion random

        #region Encrption

        public static string EncryptString(string str)
        {
            return Setting.ENCRYPT_STRING ? CryptoString.Encrypt(str) : str;
        }

        public static string DecryptString(string str)
        {
            return Setting.ENCRYPT_STRING ? CryptoString.Decrypt(str) : str;
        }

        #endregion
        
    }
}
