using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace Bale007.GoogleSheet
{
    /// <summary>
    ///     publish csv, then replace the link's sheet id with {0} to make a generic download link
    /// </summary>
    public class GoogleSheetDownloader
    {
        private readonly string onlineCsvPath;
        private readonly string saveDataPath;

        private List<CsvData> downloadList;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="onlineCsvPath">Publish google sheet as csv, then replace the sheet id part with {0}</param>
        /// <param name="saveDataPath">For example, "Assets/Data/{0}.csv" </param>
        public GoogleSheetDownloader(string onlineCsvPath, string saveDataPath)
        {
            this.onlineCsvPath = onlineCsvPath;
            this.saveDataPath = saveDataPath;
            this.downloadList = new List<CsvData>();
        }

        public IEnumerator PullData()
        {            
#if UNITY_EDITOR
            if (PlayerPrefs.GetInt(Setting.DOWNLOAD_KEY) == 1 && downloadList.Count > 0)
            {
                PlayerPrefs.SetInt(Setting.DOWNLOAD_KEY, 0);
                var newAsset = false;

                foreach (var data in downloadList)
                {
                    Debug.Log("Pulling: " + data.tableName);

                    var assetPath = string.Format(saveDataPath, data.tableName);
                    var downloadPath = string.Format(onlineCsvPath, data.sheetId);

                    var www = UnityWebRequest.Get(downloadPath);

                    yield return www.SendWebRequest();

                    if (File.Exists(assetPath) == false) newAsset = true;

                    File.WriteAllText(assetPath, www.downloadHandler.text);
                }

                Caching.ClearCache();

                AssetDatabase.Refresh();

                if (newAsset) EditorApplication.isPlaying = false;
            }
#endif
            yield return null;
        }

        public void AddItem(string sheetId, string tableName)
        {
            downloadList.Add(new CsvData(sheetId, tableName));
        }
        
        private struct CsvData
        {
            public readonly string sheetId;
            public readonly string tableName;

            public CsvData(string sheetId, string tableName)
            {
                this.sheetId = sheetId;
                this.tableName = tableName;
            }
        }
    }
}