using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Bale007.Util
{
    public static class JsonUtil
    {
        public static void Save<T>(T content, string filename) where T : class
        {
            var dataJson = JsonConvert.SerializeObject(content, Formatting.Indented);

            //TODO Encryption
            var dataEncrypted = ClientUtil.EncryptString(dataJson);

            //string filePath = Path.Combine(Constant.SAVE_PATH, Constant.SAVE_FOLDER_NAME);
            var filePath = Setting.SAVE_PATH;

            Debug.Log("root path:" + filePath);
            //createIfNotExist(filePath);

            filePath = Path.Combine(filePath, filename);

            File.WriteAllText(filePath, dataEncrypted);

            Debug.Log("----- Save Success -----");
            Debug.Log("Timestamp: " + ClientUtil.GetSystemTime());
            Debug.Log("Path: " + filePath);
            Debug.Log("Size: " + SizeOfString(dataEncrypted) + " Kb");
            Debug.Log("------------------------");
        }

        public static T Load<T>(string filename) where T : class
        {
            string filePath;

            filePath = Setting.SAVE_PATH;

            //createIfNotExist(filePath);

            filePath = Path.Combine(filePath, filename);

            if (File.Exists(filePath))
            {
                var timeBeforeSerialization = Time.realtimeSinceStartup;

                var dataEncrypted = File.ReadAllText(filePath);

                var dataDecrypted = ClientUtil.DecryptString(dataEncrypted);

                var content = JsonConvert.DeserializeObject<T>(dataDecrypted);

                var timeAfterSerialization = Time.realtimeSinceStartup;

                Debug.Log("<color=green>Loaded Save File</color>");
                Debug.Log("Path: " + filePath);
                Debug.Log("Size: " + SizeOfString(dataEncrypted) + " Kb");
                Debug.Log(
                    "Json Serialization Time: " + (timeAfterSerialization - timeBeforeSerialization) * 1000 + "ms");

                return content;
            }

            Debug.Log("<color=red>Load Failed</color>");
            Debug.Log("Path: " + filePath);

            return null;
        }

        public static void Delete(string filename)
        {
            string filePath;

            filePath = Setting.SAVE_PATH;

            filePath = Path.Combine(filePath, filename);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);

                Debug.LogFormat("<color=red>File Deleted:</color> {0}", filePath);
            }
        }

        private static float SizeOfString(string content)
        {
            return content.Length * sizeof(char) / 1024f;
        }
    }
}