using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace Bale007.GoogleSheet
{
    public class GoogleSheetDownloader
    {
        private readonly string onlineCsvPath;
        private readonly string saveDataPath;
        public GoogleSheetDownloader(string onlineCsvPath, string saveDataPath)
        {
            this.onlineCsvPath = onlineCsvPath;
            this.saveDataPath = saveDataPath;
        }

        public IEnumerator PullDataFromGoogleSheet()
        {
#if UNITY_EDITOR
            if (PlayerPrefs.GetInt(Setting.DOWNLOAD_KEY) == 1)
            {
                PlayerPrefs.SetInt(Setting.DOWNLOAD_KEY, 0);
                bool newAsset = false;
                var downloadList = MakeDownloadList();

                foreach (CsvData data in MakeDownloadList())
                {
                    Debug.Log("Downloading:" + data.tableName);

                    string assetPath = string.Format(saveDataPath, data.tableName);
                    string downloadPath = string.Format(onlineCsvPath, data.sheetId);

                    UnityWebRequest www = UnityWebRequest.Get(downloadPath);

                    yield return www.SendWebRequest();

                    if (File.Exists(assetPath) == false)
                    {
                        newAsset = true;
                    }

                    File.WriteAllText(assetPath, www.downloadHandler.text);
                }
   
                Caching.ClearCache();

                AssetDatabase.Refresh();

                if (newAsset)
                {
                    EditorApplication.isPlaying = false;
                }

            }
#endif
            yield return null;
        }

        private List<CsvData> MakeDownloadList()
        {
            var downloadList = new List<CsvData>
            {
/*                new CsvData("1346181784", nameof(TextPattern)),
                    new CsvData("196425171", "InGame" + nameof(TextPattern))*/
            };

            return downloadList;
        }

        [System.Serializable]
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
