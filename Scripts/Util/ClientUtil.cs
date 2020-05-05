using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Bale007.Crypto;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Bale007.Util
{
    public static class ClientUtil
    {
        #region Debug

        public static void ShowSystemInfo()
        {
            Debug.Log("## System memory: " + SystemInfo.systemMemorySize + " MB");
            Debug.Log("## Graphics API: " + SystemInfo.graphicsDeviceVersion);
        }

        public static void Log(string aLog)
        {
            if (Setting.SHOW_DEBUG_MSG) Debug.Log(aLog);
        }
        
        public static void LogFormat(string aLog, params object[] aParam)
        {
            if (Setting.SHOW_DEBUG_MSG) Debug.LogFormat(aLog, aParam);
        }

        public static void LogErrorFormat(string aLog, params object[] aParam)
        {
            if (Setting.SHOW_DEBUG_MSG) Debug.LogErrorFormat(aLog, aParam);
        }
        
        public static void LogError(string aLog)
        {
            if (Setting.SHOW_DEBUG_MSG) Debug.LogError(aLog);
        }

        public static void Log(string aLog, string color)
        {
            if (Setting.SHOW_DEBUG_MSG) Debug.Log("<color=" + color + ">" + aLog + "</color>");
        }

        #endregion debug

        #region Parsing

        public static T ParseEnum<T>(string value) where T : Enum
        {
            try
            {
                return (T) Enum.Parse(typeof(T), value);
            }
            catch
            {
                throw new Exception("Error Happended When Parsing:" + value);
            }
        }

        public static int ParseInt(string aText)
        {
            return int.Parse(aText, NumberFormatInfo.InvariantInfo);
        }

        public static float ParseFloat(string aText)
        {
            return float.Parse(aText, NumberFormatInfo.InvariantInfo);
        }

        public static Vector2 ParseVector2(string aText)
        {
            var textSplit = aText.Split(',');
            return new Vector2(ParseFloat(textSplit[0]), ParseFloat(textSplit[1]));
        }

        public static string ToUpper(string aText)
        {
            return aText.ToUpper(CultureInfo.InvariantCulture);
        }

        public static string ToLower(string aText)
        {
            return aText.ToLower(CultureInfo.InvariantCulture);
        }

        public static int CompareTo(string a, string b)
        {
            return string.Compare(a, b, StringComparison.Ordinal);
        }

        #endregion parsing

        #region Conversion

        public static double ConvertBytesToMegabytes(long bytes)
        {
            return bytes / 1024f / 1024f;
        }

        public static long ConvertMegabytesToBytes(long megabytes)
        {
            return megabytes * 1024 * 1024;
        }

        public static string ListToString(List<string> content, char separator = ',')
        {
            var result = "";

            if (content == null) return null;

            for (var i = 0; i < content.Count; i++)
            {
                if (i != 0) result += ",";

                result += content[i];
            }

            return result;
        }

        public static string ListToString(List<int> content, char separator = ',')
        {
            var result = "";

            if (content == null) return null;

            for (var i = 0; i < content.Count; i++)
            {
                if (i != 0) result += ",";

                result += content[i];
            }

            return result;
        }

        public static List<string> StringToList(string content, char separator = ',')
        {
            if (string.IsNullOrEmpty(content)) return null;

            if (!content.Contains(separator)) return new List<string> {content};

            return new List<string>(content.Split(separator));
        }

        public static List<int> StringToIntegerList(string content, char separator = ',')
        {
            if (string.IsNullOrEmpty(content)) return null;

            if (!content.Contains(separator)) return new List<int> {int.Parse(content)};

            var list = new List<int>();

            foreach (var value in content.Split(separator)) list.Add(int.Parse(value));

            return list;
        }

        public static List<T> StringToList<T>(string content, char separator = ',') where T : Enum
        {
            if (string.IsNullOrEmpty(content)) return null;

            if (!content.Contains(separator)) return new List<T> {ParseEnum<T>(content)};

            var list = new List<T>();

            foreach (var str in StringToList(content)) list.Add(ParseEnum<T>(str));

            return list;
        }

        public static string ColorToHex(Color32 color)
        {
            var hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
            return hex;
        }

        public static Color HexToColor(string hex)
        {
            hex = hex.Replace("0x", ""); //in case the string is formatted 0xFFFFFF
            hex = hex.Replace("#", ""); //in case the string is formatted #FFFFFF
            byte a = 255; //assume fully visible unless specified in hex
            var r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
            var g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
            var b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
            //Only use alpha if the string has enough characters
            if (hex.Length == 8) a = byte.Parse(hex.Substring(6, 2), NumberStyles.HexNumber);

            return new Color32(r, g, b, a);
        }

        public static List<string> DictionaryToList(Dictionary<string, int> dictionary, char separator = ':')
        {
            var result = new List<string>();

            foreach (var pair in dictionary) result.Add(pair.Key + separator + pair.Value);

            return result;
        }

        public static List<string> DictionaryToList(Dictionary<int, int> dictionary, char separator = ':')
        {
            var result = new List<string>();

            foreach (var pair in dictionary) result.Add(pair.Key.ToString() + separator + pair.Value);

            return result;
        }

        public static List<string> DictionaryToList<T>(Dictionary<T, int> dictionary, char separator = ':')
            where T : Enum
        {
            var result = new List<string>();

            foreach (var pair in dictionary) result.Add(pair.Key.ToString() + separator + pair.Value);

            return result;
        }

        public static Dictionary<string, int> StringToIntDictionary(string content, char rowChar = '&',
            char pairChar = '=')
        {
            if (string.IsNullOrEmpty(content)) return null;

            var returnDic = new Dictionary<string, int>();

            var rowString = StringToList(content, rowChar);

            foreach (var rowData in rowString)
            {
                var arr = rowData.Split(pairChar);

                returnDic.Add(arr[0], int.Parse(arr[1]));
            }

            return returnDic;
        }

        public static Dictionary<T, int> StringToIntDictionary<T>(string content, char rowChar = '&',
            char pairChar = '=') where T : Enum
        {
            if (string.IsNullOrEmpty(content)) return null;

            var returnDic = new Dictionary<T, int>();

            var rowString = StringToList(content, rowChar);

            foreach (var rowData in rowString)
            {
                var arr = rowData.Split(pairChar);

                returnDic.Add(ParseEnum<T>(arr[0]), int.Parse(arr[1]));
            }

            return returnDic;
        }

        public static Dictionary<string, string> StringToStringDictionary(string content, char rowChar = '&',
            char pairChar = '=')
        {
            if (string.IsNullOrEmpty(content)) return null;

            var returnDic = new Dictionary<string, string>();

            var rowString = StringToList(content, rowChar);

            foreach (var rowData in rowString)
            {
                var arr = rowData.Split(pairChar);

                returnDic.Add(arr[0], arr[1]);
            }

            return returnDic;
        }

        public static Dictionary<int, string> StringToIntStringDictionary(string content, char rowChar = '&',
            char pairChar = '=')
        {
            if (string.IsNullOrEmpty(content)) return null;

            var returnDic = new Dictionary<int, string>();

            var rowString = StringToList(content, rowChar);

            foreach (var rowData in rowString)
            {
                var arr = rowData.Split(pairChar);

                returnDic.Add(int.Parse(arr[0]), arr[1]);
            }

            return returnDic;
        }

        public static Dictionary<T, string> StringToStringDictionary<T>(string content, char rowChar = '&',
            char pairChar = '=') where T : Enum
        {
            if (string.IsNullOrEmpty(content)) return null;

            var returnDic = new Dictionary<T, string>();

            var rowString = StringToList(content, rowChar);

            foreach (var rowData in rowString)
            {
                var arr = rowData.Split(pairChar);

                returnDic.Add(ParseEnum<T>(arr[0]), arr[1]);
            }

            return returnDic;
        }

        public static Dictionary<string, T> StringToEnumDictionary<T>(string content, char rowChar = '&',
            char pairChar = '=') where T : Enum
        {
            if (string.IsNullOrEmpty(content)) return null;

            var returnDic = new Dictionary<string, T>();

            var rowString = StringToList(content, rowChar);

            foreach (var rowData in rowString)
            {
                var arr = rowData.Split(pairChar);

                returnDic.Add(arr[0], ParseEnum<T>(arr[1]));
            }

            return returnDic;
        }

        public static Dictionary<string, string> ListToStringDictionary(List<string> list, char separator = ':')
        {
            var aDict = new Dictionary<string, string>();

            foreach (var content in list)
            {
                var values = content.Split(separator);

                aDict.Add(values[0], values[1]);
            }

            return aDict;
        }

        public static Dictionary<T, string> ListToStringDictionary<T>(List<string> list, char separator = ':')
            where T : Enum
        {
            var aDict = new Dictionary<T, string>();

            foreach (var content in list)
            {
                var values = content.Split(separator);

                aDict.Add(ParseEnum<T>(values[0]), values[1]);
            }

            return aDict;
        }

        public static Dictionary<string, int> ListToStringIntDictionary(List<string> list, char separator = ':')
        {
            var aDict = new Dictionary<string, int>();

            foreach (var content in list)
            {
                var values = content.Split(separator);

                aDict.Add(values[0], int.Parse(values[1]));
            }

            return aDict;
        }

        public static Dictionary<int, int> ListToIntIntDictionary(List<string> list, char separator = ':')
        {
            var aDict = new Dictionary<int, int>();

            foreach (var content in list)
            {
                var values = content.Split(separator);

                aDict.Add(int.Parse(values[0]), int.Parse(values[1]));
            }

            return aDict;
        }

        #endregion conversion

        #region Display

        public static bool LayerInLayerMask(int layer, LayerMask layerMask)
        {
            if (((1 << layer) & layerMask) != 0)
                return true;
            return false;
        }

        public static int LayerMaskToLayer(LayerMask layerMask)
        {
            var layerNumber = 0;
            var layer = layerMask.value;
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
            var outputString = aName.Replace("[", "");
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
            foreach (var entry in parameters) str = str.Replace(string.Format("{{{0}}}", entry.Key), entry.Value);

            return str;
        }

        public static float Round(float value, int digits)
        {
            var mult = Mathf.Pow(10.0f, digits);
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
            var range = new List<int>();
            if (variance > 0)
            {
                var min = Mathf.RoundToInt(baseDmg * (1f - variance));
                var max = Mathf.RoundToInt(baseDmg * (1f + variance));

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
            var lowest = float.MaxValue;
            var lowestIndex = -1;
            var checkIndex = 0;
            foreach (var value in valueList)
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
            var resultDim = Mathf.RoundToInt(scaledRefDim / (float) origRefDim * origDim);

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
            var sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
            var cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

            var tx = v.x;
            var ty = v.y;
            v.x = cos * tx - sin * ty;
            v.y = sin * tx + cos * ty;
            return v;
        }

        public static Vector3 SnapVector(Vector3 v3, float snapAngle)
        {
            var angle = Vector3.Angle(v3, Vector3.up);
            if (angle < snapAngle / 2.0f)
                return Vector3.up * v3.magnitude;
            if (angle > 180.0f - snapAngle / 2.0f)
                return Vector3.down * v3.magnitude;

            var t = Mathf.Round(angle / snapAngle);
            var deltaAngle = t * snapAngle - angle;

            var axis = Vector3.Cross(Vector3.up, v3);
            var q = Quaternion.AngleAxis(deltaAngle, axis);
            return q * v3;
        }

        #endregion vectors

        #region Time

        public static string GetSystemTime()
        {
            var dt = DateTime.Now;
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
            var dt2 = DateTime.Parse(to);

            return GetTimeSpanFromNow(dt2);
        }

        public static TimeSpan GetTimeSpanFromNow(DateTime to)
        {
            var dt1 = DateTime.Now;

            var dt2 = to;

            var ts1 = new TimeSpan(dt1.Ticks);

            var ts2 = new TimeSpan(dt2.Ticks);

            var tsSub = ts1.Subtract(ts2).Negate();

            return tsSub;
        }

        public static string GetTimeFormat(int aTime)
        {
            var hr = aTime / 3600;

            aTime -= hr * 3600;

            var min = aTime / 60;

            aTime -= min * 60;

            var hrString = hr < 10 ? "0" + hr : hr.ToString();
            var minString = min < 10 ? "0" + min : min.ToString();
            var secString = aTime < 10 ? "0" + aTime : aTime.ToString();

            return hrString + ":" + minString + ":" + secString;
        }

        public static string GetHrFormat(int aHr)
        {
            if (aHr < 10)
                return "0" + aHr + ":00";
            return aHr + ":00";
        }

        public static string GetMissionTimeFormat(float aMTime)
        {
            var tString = "";
            if (aMTime < 10f)
                tString += "0" + Mathf.FloorToInt(aMTime);
            else
                tString += Mathf.FloorToInt(aMTime).ToString();

            var minInt = Mathf.FloorToInt(aMTime % 1f * 60f);
            if (minInt < 10f)
                tString += "0" + minInt;
            else
                tString += minInt.ToString();

            return tString;
        }

        #endregion Time

        #region Random

        public static T RandomEnumValue<T>() where T : Enum
        {
            var values = Enum.GetValues(typeof(T));
            var random = Random.Range(0, values.Length);
            return (T) values.GetValue(random);
        }

        public static T RandomFromList<T>(List<T> aList)
        {
            return aList[Random.Range(0, aList.Count)];
        }

        public static int RandomNegativePositive()
        {
            return Random.Range(0, 2) * 2 - 1;
        }

        public static Vector2 RandomNormalizedVector()
        {
            var v = Vector2.one;
            v.x = Random.Range(-1f, 1f);
            v.y = Random.Range(-1f, 1f);
            return v.normalized;
        }

        public static bool ChanceSuccess100(float chance)
        {
            return chance > Random.Range(0, 100);
        }

        public static bool RandomizeChance(float chance)
        {
            if (chance <= 0) return false;

            if (chance >= 1f) return true;

            var randomFloat = Random.Range(0, 1f);
            if (randomFloat > chance)
                return false;
            return true;
        }

        public static List<int> ShuffleIntList(List<int> toShuffle, int selectCount)
        {
            var shuffleIndexList = GetRandomIntList(toShuffle.Count, selectCount);
            var shuffledList = new List<int>();
            foreach (var shuffleIndex in shuffleIndexList) shuffledList.Add(toShuffle[shuffleIndex]);

            return shuffledList;
        }

        public static List<int> SelectRandomIntList(List<int> intList, int selectCount)
        {
            var selectList = new List<int>();
            var indexList = GetRandomIntList(intList.Count, selectCount);

            foreach (var randInd in indexList) selectList.Add(intList[randInd]);

            return selectList;
        }

        public static int ChooseRandomIndex(int listCount)
        {
            return Random.Range(0, listCount);
        }

        public static List<int> GetRandomIntList(int listCount, int selectCount)
        {
            var selectList = new List<int>();
            var randomList = new List<int>();

            selectCount = Math.Min(listCount, selectCount);

            for (var i = 0; i < listCount; i++) randomList.Add(i);

            while (randomList.Count > 0 && selectCount > 0)
            {
                var randomIndex = Random.Range(0, randomList.Count);
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