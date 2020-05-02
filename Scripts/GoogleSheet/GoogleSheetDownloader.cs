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

        public GoogleSheetDownloader(string onlineCsvPath, string saveDataPath)
        {
            this.onlineCsvPath = onlineCsvPath;
            this.saveDataPath = saveDataPath;
        }

        public IEnumerator PullData()
        {
#if UNITY_EDITOR
            if (PlayerPrefs.GetInt(Setting.DOWNLOAD_KEY) == 1)
            {
                PlayerPrefs.SetInt(Setting.DOWNLOAD_KEY, 0);
                var newAsset = false;
                var downloadList = MakeDownloadList();

                foreach (var data in MakeDownloadList())
                {
                    Debug.Log("Downloading:" + data.tableName);

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

        private List<CsvData> MakeDownloadList()
        {
            var downloadList = new List<CsvData>();

            return downloadList;
        }

        [Serializable]
        private struct CsvData
        {
            public string sheetId;
            public string tableName;

            public CsvData(string sheetId, string tableName)
            {
                this.sheetId = sheetId;
                this.tableName = tableName;
            }
        }
    }
}