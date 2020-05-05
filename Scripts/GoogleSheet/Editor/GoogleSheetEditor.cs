using UnityEditor;
using UnityEngine;

namespace Bale007.Editor
{
    public static class GoogleSheetEditor
    {
        [MenuItem("Google Sheet/Data/Set Pull")]
        public static void SetPullData()
        {
            PlayerPrefs.SetInt(Setting.DOWNLOAD_KEY, 1);

            Debug.Log("Pull Data [1]");
        }
        
        [MenuItem("Google Sheet/Data/Cancel Pull")]
        public static void CancelPullData()
        {
            PlayerPrefs.SetInt(Setting.DOWNLOAD_KEY, 0);

            Debug.Log("Pull Data [0]");
        }
    }
}